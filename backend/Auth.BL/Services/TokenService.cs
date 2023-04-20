using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AuthInterfaces;
using Microsoft.IdentityModel.Tokens;

namespace Auth.BL.Services {
    public class TokenService : ITokenService {
        public string GenerateAccessToken(IEnumerable<Claim> claims) {
            var secretKey = JwtConfiguration.GetSymmetricSecurityKey();
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: JwtConfiguration.Issuer,
                audience: JwtConfiguration.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public string GenerateRefreshToken() {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create()) {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token) {
            var tokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidIssuer = JwtConfiguration.Issuer,
                ValidateAudience = true,
                ValidAudience = JwtConfiguration.Audience,
                ValidateLifetime = false,
                IssuerSigningKey = JwtConfiguration.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
            };
            try {
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;


                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");
				return principal;
			}
            catch(Exception ex) {
				throw new SecurityTokenException("Invalid token format");

			}
            
        }
    }
}
