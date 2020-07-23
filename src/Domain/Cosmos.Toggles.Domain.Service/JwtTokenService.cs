using Cosmos.Toggles.Domain.DataTransferObject;
using Cosmos.Toggles.Domain.Service.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Toggles.Domain.Service
{
    public class JwtTokenService : ITokenService
    {
        public Task<string> CreateJwtAsync(string userId, string userName, string userEmail, int expiresSeconds)
        {
            // TODO: Get secret by azure key vault.
            var key = Encoding.ASCII.GetBytes("eaa1e32b-8a8e-45ac-bfc5-e6a62078c2e5");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Email, userEmail)
                }),
                Expires = DateTime.UtcNow.AddMinutes(25),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Task.FromResult<string>(tokenHandler.WriteToken(token));
        }

        public Task<string> CreateKeyAsync()
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);

                return Task.FromResult<string>(Convert.ToBase64String(randomBytes));
            }
        }

        public Task<User> ExtractUserAsync(string jwt)
        {
            if (!string.IsNullOrEmpty(jwt))
            {
                var tokenHandler = new JwtSecurityTokenHandler { };
                var jwtSecurityToken = tokenHandler.ReadJwtToken(jwt);

                if (jwtSecurityToken != null && !string.IsNullOrEmpty(jwtSecurityToken.RawPayload))
                {
                    var length = jwtSecurityToken.RawPayload.Length;
                    var payload = jwtSecurityToken.RawPayload.PadRight(length + ((length % 4 != 0) ? (4 - length % 4) : 0), '=');
                    var josn = Encoding.ASCII.GetString(Convert.FromBase64String(payload));

                    dynamic claims = JObject.Parse(josn);
                    return Task.FromResult<User>(new User { Id = claims.nameid, Name = claims.unique_name, Email = claims.email });
                }
            }

            return null;
        }
    }
}
