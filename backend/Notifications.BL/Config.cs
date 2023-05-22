using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Notifications.BL {
	public class Config {
		private static IConfigurationRoot configuration = new ConfigurationBuilder()
		   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
		   .AddJsonFile("appsettings.json")
		   .Build();


		public static string Issuer = configuration.GetSection("JwtSettings")["Issuer"];
		public static string Audience = configuration.GetSection("JwtSettings")["Audience"];
		public static string LifeTime = configuration.GetSection("JwtSettings")["Lifetime"];
		private static string Key = configuration.GetSection("JwtSettings")["Key"];
		public static string QueueName = configuration.GetSection("QueueSettings")["QueueName"];
		public static string HostName = configuration.GetSection("QueueSettings")["HostName"];

		public static SymmetricSecurityKey GetSymmetricSecurityKey() {
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
		}
	}
}
