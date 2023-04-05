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
    public static class DependencyAuthDbExt {
        public static IServiceCollection AddAuthBlDependency(this IServiceCollection services) {
            services.AddDbContext<AuthDbContext>();

            return services;

        }
    }
    
    
}
