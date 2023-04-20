using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Auth.DAL.Data;
using Auth.DAL.Data.Entities;
using AuthInterfaces;
using AutoMapper;
using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Auth.BL.Services {
	public class AccountService : IAccountService {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;
        private readonly AuthDbContext _ctx;
        private readonly ITokenService _tokenService;
        public AccountService(UserManager<User> userManager, IMapper mapper,
                AuthDbContext ctx,
                ITokenService tokenService,
                SignInManager<User> signInManager,ILogger<AccountService> logger) {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
            _ctx= ctx;
            _tokenService= tokenService;
          }
        public async Task<AuthenticatedResponse> Register(RegisterModelDTO RegisterModel) {

            if (RegisterModel == null) throw new ArgumentNullException(nameof(RegisterModel));

            if (RegisterModel.Email == null) {
                throw new ArgumentNullException(nameof(RegisterModel.Email));
            }

            if (RegisterModel.Password == null) {
                throw new ArgumentNullException(nameof(RegisterModel.Password));
            }

            if (await _userManager.FindByEmailAsync(RegisterModel.Email) != null)
                throw new ArgumentException("User with this email already exists");

            var user = _mapper.Map<User>(RegisterModel);
            
            var result = await _userManager.CreateAsync(user, RegisterModel.Password); // Создание нового пользователя в системе с указанными данными и введенным паролем
            if (result.Succeeded) // результат может быть успешным, может также возникнуть ошибка, если был введен пароль, не отвечающий требованиям
            {

                return await Login(new LoginCredentials {
                    Email = RegisterModel.Email, Password = RegisterModel.Password 
                });
            }

            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new InvalidOperationException(errors);
        }
        public async Task<Response> EditUserToCustomer (string address, string userName) {
            if (string.IsNullOrEmpty(address)) {
                throw new ArgumentNullException(nameof(address));
            }
            var NewUser = await _userManager.FindByEmailAsync(userName);
           var result = await _userManager.AddToRoleAsync(NewUser, ApplicationRoleNames.Customer);
            if (result.Succeeded) {
                NewUser.Customer = new Customer() {
                    Id = NewUser.Id,
                    Address = address,
                };
                await _userManager.UpdateAsync(NewUser);
                return new Response() {
                    Message = "succesfully added",
                    Status = "200"
                };
            }
            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new InvalidOperationException(errors);
        }
        public async Task<AuthenticatedResponse> Login(LoginCredentials loginCredentials) {


            if (loginCredentials == null) {
                throw new ArgumentNullException(nameof(loginCredentials));
            }
            var user = await _userManager.Users.Include(u => u.Customer).FirstOrDefaultAsync(u => u.Email == loginCredentials.Email);
            if (user == null) {
                throw new ArgumentException("wrong email or password");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginCredentials.Password, false);

			if (result.IsLockedOut)
				throw new InvalidOperationException("Your account is locked out");
			if (!result.Succeeded) 
            throw new InvalidOperationException("wrong email or password");


            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            foreach (var role in await _userManager.GetRolesAsync(user)) {
                claims.Add(new Claim(ClaimTypes.Role, role));
                if (role == ApplicationRoleNames.Customer) {
                    claims.Add(new Claim(ClaimTypes.StreetAddress, user.Customer.Address.ToString()));
                }
            }

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
           await _userManager.UpdateAsync(user);

            return (new AuthenticatedResponse {
                Email = loginCredentials.Email,
                Token = accessToken,
                RefreshToken = refreshToken,
                Role = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList(),
            });

           
        }
        

        public async Task<ProfileDTO> GetProfile(string userName) {
            var user = await _userManager.Users.Include(u=>u.Customer).FirstOrDefaultAsync(x=>x.UserName == userName);
            if (user == null) throw new KeyNotFoundException("User not found");
            if (user.Customer != null) {
                user = _userManager.Users.Where(u => u.Email == userName).
                    Include(u => u.Roles).ThenInclude(r => r.Role).Include(u => u.Customer)
                    .First();
            }
            else user = _userManager.Users.Where(u => u.Email == userName).
                    Include(u => u.Roles).ThenInclude(r => r.Role)
                    .First();

            _logger.LogInformation("User`s profile was returned successfuly");
            var model = _mapper.Map<ProfileDTO>(user);
            return model;
        }

        public async Task<Response> EditProfile(EditProfileDTO model, string userName) {
            var user = await _userManager.Users.Include(u=>u.Customer).FirstOrDefaultAsync(u =>u.UserName == userName);
            if (user == null) throw new KeyNotFoundException("User not found");

            user.FullName = Regex.Replace(model.FullName, @"\s+", " ");
            user.BirthDate = model.BitrhDate;
            user.Gender = model.Gender;
            user.PhoneNumber= model.PhoneNumber;
            if (user.Customer != null && model.Address != null) { user.Customer.Address = model.Address; }
           var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) {
                return new Response {
                    Status = "Ok",
                    Message = "User successfully modified"
                };
            }
            else {
                var errors = string.Join(", ", result.Errors.Select(x => x.Description));
                throw new InvalidOperationException(errors);
            }
            
        }

        public async Task<AuthenticatedResponse> Refresh(TokenApiModel tokenApiModel) {
            if (tokenApiModel is null) throw new ArgumentNullException();
            if (tokenApiModel.RefreshToken is null) throw new ArgumentNullException();
            if (tokenApiModel.AccessToken is null) throw new ArgumentNullException();

            var principal = _tokenService.GetPrincipalFromExpiredToken(tokenApiModel.AccessToken);
            var userName = principal.Identity.Name;

            var user = await _userManager.Users.Include(r=>r.Roles).ThenInclude(r=>r.Role).FirstOrDefaultAsync(u=>u.UserName == userName);
            if (user is null)
                throw new KeyNotFoundException("User with this token does not exist");
            if (user.RefreshToken != tokenApiModel.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                throw new InvalidOperationException("Wrong or expired refresh token");

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            /*var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            var result = await _userManager.UpdateAsync(user);*/

            return new AuthenticatedResponse() {
                Token = newAccessToken,
                RefreshToken = tokenApiModel.RefreshToken,
                Email = user.UserName,
                Role = user.Roles.IsNullOrEmpty() ? new List<string>() : user.Roles.Select(c => c.Role.ToString()).ToList()
              };
            
           
        }

        public async Task<Response> Logout(string userName) {
           var user = await _userManager.FindByEmailAsync(userName);
            if (user is null ) throw new KeyNotFoundException("User not found");
            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);
            return new Response() {
                Status = "200",
                Message = "Succesfully log out"
            };
        }

        public async Task<Response> ChangePassword(string userName, ChangePasswordModelDTO model) {
            var user = await _userManager.FindByEmailAsync(userName);
            if (user is null) throw new KeyNotFoundException("User not found");
           var result = await _userManager.ChangePasswordAsync(user, model.OldPassword,model.Password);
            if (result.Succeeded) {
                return new Response() {
                    Status = "200",
                    Message = "Password succesfully changed"
                };
            }
            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new InvalidOperationException(errors);
            
        }
    }
}
