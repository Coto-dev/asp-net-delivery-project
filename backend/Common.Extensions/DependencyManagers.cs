using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.DAL.Data.Entities;
using Auth.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions {
    public static class DependencyManagers {
        public static IServiceCollection AddIdentityManagers(this IServiceCollection services) {

            services.AddIdentity<User, Role>(options => { options.SignIn.RequireConfirmedEmail = false && options.User.RequireUniqueEmail; })
                        .AddEntityFrameworkStores<AuthDbContext>()
                        .AddDefaultTokenProviders()
                        .AddSignInManager<SignInManager<User>>()
                        .AddUserManager<UserManager<User>>();

            return services;

        }
    }
}
