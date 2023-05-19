using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.DAL.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Common.Extensions {
    public static class DependencyAuthDbExt {
		public static IServiceCollection AddAuthContext(this IServiceCollection services, IConfiguration configuration) {
			services.AddDbContext<AuthDbContext>(
				options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionAuth"))
				);
			return services;

		}
	
    }
    
    
}
