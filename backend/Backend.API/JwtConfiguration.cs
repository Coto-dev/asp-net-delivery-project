using jwtConfiguration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Backend.API {
	public class JwtConfiguration {
		private static IConfigurationRoot configuration = new ConfigurationBuilder()
			.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
			.AddJsonFile("appsettings.json")
			.Build();

		public static string Issuer = jwtConfig.Issuer;
		public static string Audience = jwtConfig.Audience;
		private static string Key = jwtConfig.Key;
		// private static string key = jwtConfig.Key;
		public static SymmetricSecurityKey GetSymmetricSecurityKey() {
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
		}
	}
}
