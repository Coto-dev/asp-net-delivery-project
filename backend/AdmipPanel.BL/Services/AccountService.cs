using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Auth.DAL.Data;
using Auth.DAL.Data.Entities;
using Backend.DAL.Data;
using Common.AdmipPanelInterfaces;
using Common.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AdmipPanel.BL.Services {
    public class AccountService : IAccountService {
        private readonly ILogger<AccountService> _logger;
        private readonly BackendDbContext _contextBackend;
        private readonly AuthDbContext _contextAuth;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountService(ILogger<AccountService> logger, BackendDbContext context, 
            AuthDbContext authDb, SignInManager<User> signInManager,
            UserManager<User> userManager) {
            _logger = logger;
            _contextBackend = context;
            _contextAuth = authDb;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task Login(LoginCredentials model) {
            var user = await _userManager.FindByNameAsync(model.Email); 
            if (user == null) {
                throw new KeyNotFoundException($"User with email = {model.Email} does not found");
            }
			await _signInManager.SignOutAsync();
			var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);

			if (result.IsLockedOut)
				throw new ArgumentException("Your account is locked out.");

			var claims = new List<Claim>
            {
                new ("Name", user.UserName),
                new ("Id", user.Id.ToString()),
                new (ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            if (user.Roles?.Any() == true) {
                var roles = user.Roles.Select(x => x.Role).ToList();
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
            }

            var authProperties = new AuthenticationProperties {
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(2),
                IsPersistent = true
            };

            await _signInManager.SignInWithClaimsAsync(user, authProperties, claims);
        }

        public async Task Logout() {
            await _signInManager.SignOutAsync();
        }


    }
}
