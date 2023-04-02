using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.BL.Services;
using Backend.DAL.Data.Entities;
using Common.BackendInterfaces;
using Microsoft.Extensions.DependencyInjection;
namespace Backend.BL.Extensions {
   

    public static class ServiceProviderExtensions {
        public static void AddDishService(this IServiceCollection services) {
            services.AddScoped<IDishService,DishService>();
            services.AddScoped<IRestarauntService, RestarauntService>();
        }
    }
}
