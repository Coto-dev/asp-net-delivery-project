using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jwtConfiguration;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.BL {
    public class JwtConfiguration {
        private static IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        /*public static int Lifetime = configuration.GetSection("JwtConfiguration").GetValue<int>("LifetimeMinutes");
        public static string Issuer = configuration.GetSection("JwtConfiguration").GetValue<string>("Issuer");
        public static string Audience = configuration.GetSection("JwtConfiguration").GetValue<string>("Audience");
        private static string Key = configuration.GetSection("JwtConfiguration").GetValue<string>("Key");*/

        public static int Lifetime = jwtConfig.Lifetime;
        public static string Issuer = jwtConfig.Issuer;
        public static string Audience = jwtConfig.Audience;
        private static string Key = jwtConfig.Key;
        public static SymmetricSecurityKey GetSymmetricSecurityKey() {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
   
}
