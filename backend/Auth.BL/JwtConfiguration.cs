using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Auth.BL
{
    public class JwtConfiguration
    {
        private static IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();


		public static string Issuer = configuration.GetSection("JwtSettings")["Issuer"];
		public static string Audience = configuration.GetSection("JwtSettings")["Audience"];
		public static string LifeTime = configuration.GetSection("JwtSettings")["Lifetime"];
		private static string Key = configuration.GetSection("JwtSettings")["Key"];


		public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }

}
