using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.BL;
using Auth.BL.Data.Entities;
using AutoMapper;
using Common.AuthInterfaces;
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
        public AccountService(UserManager<User> userManager, IMapper mapper,
                SignInManager<User> signInManager,ILogger<AccountService> logger) {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<TokenResponse> Register(RegisterModelDTO RegisterModel) {
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
                await _userManager.AddToRoleAsync(user, ApplicationRoleNames.Customer);

            }

            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new ArgumentException(errors);
        }
        public async Task<TokenResponse> Login(LoginCredentials loginCredentials) {
            var identity = await GetIdentity(loginCredentials.Email.ToLower(), loginCredentials.Password);
            if (identity == null) {
                throw new ArgumentException("Incorrect username or password");
            }

            var user = _userManager.Users.Where(x => x.Email == loginCredentials.Email).First();

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: JwtConfiguration.Issuer,
                audience: JwtConfiguration.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(JwtConfiguration.Lifetime)),
                signingCredentials: new SigningCredentials(JwtConfiguration.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));

            _logger.LogInformation("Successful login");

            return new TokenResponse {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                Email = identity.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault(""),
                Role = identity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList(),
              
            };
        }
        private async Task<ClaimsIdentity?> GetIdentity(string email, string password) {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            if (!result.Succeeded) return null;

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, user.Email ?? "")
            };

            foreach (var role in await _userManager.GetRolesAsync(user)) {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return new ClaimsIdentity(claims, "Token", ClaimTypes.Name, ClaimTypes.Role);
        }
    }
}
