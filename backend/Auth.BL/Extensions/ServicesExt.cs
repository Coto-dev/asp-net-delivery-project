using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.BL.Services;
using Common.AuthInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.BL.Extensions {
    public static class ServiceProviderExtensions {
        public static void AddAccountService(this IServiceCollection services) {
            services.AddScoped<IAccountService,AccountService>();
        }
    }
}
