using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.BL.Data.Entities;
using Auth.BL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.BL.Extensions {
    public static class DependencyIdentityExt {
        public static IServiceCollection AddAuthBlIdentityDependency(this IServiceCollection services) {

            services.AddIdentity<User, Role>(options => { options.SignIn.RequireConfirmedEmail = false; })
                        .AddEntityFrameworkStores<AuthDbContext>()
                        .AddDefaultTokenProviders()
                        .AddSignInManager<SignInManager<User>>()
                        .AddUserManager<UserManager<User>>()
                        .AddRoleManager<RoleManager<Role>>();

            return services;

        }
    }
}
