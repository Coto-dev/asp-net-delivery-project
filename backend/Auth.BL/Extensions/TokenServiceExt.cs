using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.BL.Services;
using AuthInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.BL.Extensions {
    public static class TokenServiceExt {
        public static void AddTokenService(this IServiceCollection services) {
            services.AddTransient<ITokenService, TokenService>();

        }
    }
}
