using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DAL.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.BL.Extensions {
    public static class ServiceDependencyExtension {
        public static IServiceCollection AddBackendBlServiceDependencies(this IServiceCollection services) {
            services.AddDbContext<BackendDbContext>();
            return services;
        }
    }
}
