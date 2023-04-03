using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.DAL.Data.Entities;
using Auth.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.DAL.Extensions {
    public static class DependencyAuthDbExt {
        public static IServiceCollection AddAuthDependencyIdentity(this IServiceCollection services) {
            services.AddDbContext<AuthDbContext>();
            services.AddIdentity<User, Role>() // Добавление identity к проекту
                            .AddEntityFrameworkStores<AuthDbContext>() // указание контекста
                            .AddSignInManager<SignInManager<User>>() // явное указание того, что менеджер авторизации должен работать с переопределенной моделью пользователя
                            .AddUserManager<UserManager<User>>() // аналогично для менеджера юзеров
                            .AddRoleManager<RoleManager<Role>>(); // аналогично для менеджера ролей
            return services;
        }
    }
    
    
}
