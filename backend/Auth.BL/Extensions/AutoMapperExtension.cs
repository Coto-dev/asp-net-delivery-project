using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.BL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.BL.Extensions {
    public static class AutoMapperExtension {
        public static void AddAutoMapperExt(this IServiceCollection services) {
            services.AddAutoMapper(typeof(MappingProfile));

        }
    }
}
