using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.DAL.Data;
using Backend.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions {
    public static class ServiceDependencyExtension {
		public static IServiceCollection AddBackendContext(this IServiceCollection services, IConfiguration configuration) {
			services.AddDbContext<BackendDbContext>(
				options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionBackend"))
				);
			return services;

		}
	}
}
