using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.BL.Extensions {
    public static class ServiceDependencyExtension {
		public static IServiceCollection AddBackendDbServiceDependency(this IServiceCollection services, IConfiguration configuration) {
			services.AddDbContext<BackendDbContext>(
				options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
				);
			return services;

		}
	}
}
