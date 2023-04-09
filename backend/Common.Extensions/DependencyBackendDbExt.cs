using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DAL.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions {
    public static class ServiceDependencyExtension {
        public static IServiceCollection AddBackendContext(this IServiceCollection services) {
            services.AddDbContext<BackendDbContext>();
            return services;
        }
    }
}
