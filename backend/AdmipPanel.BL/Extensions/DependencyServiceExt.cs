﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdmipPanel.BL.Services;
using Common.AdminPanelInterfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AdmipPanel.BL.Extensions {
	public static class DependencyServiceExt {
        public static void AddAccountServiceDependency(this IServiceCollection services) {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IRestarauntService, RestarauntService>();
			services.AddScoped<IUserManagerService, UserManagerService>();
			services.AddAutoMapper(typeof(MappingProfile));

        }
    }

}
