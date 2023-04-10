using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions {
    public static class DependencyAuthDbExt {
        public static IServiceCollection AddAuthContext(this IServiceCollection services) {
            services.AddDbContext<AuthDbContext>();

            return services;

        }
    }
    
    
}
