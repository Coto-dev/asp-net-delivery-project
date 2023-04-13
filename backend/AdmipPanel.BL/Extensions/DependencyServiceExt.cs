using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdmipPanel.BL.Services;
using Common.AdminPanelInterfaces;
using Common.AdmipPanelInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AdmipPanel.BL.Extensions {
    public static class DependencyServiceExt {
        public static void AddAccountServiceDependency(this IServiceCollection services) {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICrudService, CrudService>();
            services.AddAutoMapper(typeof(MappingProfile));

        }
    }

}
