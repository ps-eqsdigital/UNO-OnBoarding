using Data.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Utils
{
    public class TokenUtils
    {
        public static string CreateAuthenticationToken(User user)
        {
            IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name!),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:Token").Value!));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );
            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public static string GeneratePasswordResetToken(int tokenLength = 20)
        {
            const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] tokenData = new byte[tokenLength];
                char[] tokenChars = new char[tokenLength];

                rng.GetBytes(tokenData);

                for (int i = 0; i < tokenLength; i++)
                {
                    int index = tokenData[i] % validChars.Length;
                    tokenChars[i] = validChars[index];
                }

                return new string(tokenChars);
            }
        }
    }
}
