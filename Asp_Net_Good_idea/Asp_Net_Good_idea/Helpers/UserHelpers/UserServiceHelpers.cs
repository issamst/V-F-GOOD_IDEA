using Asp_Net_Good_idea.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;
using Asp_Net_Good_idea.Dto.Jwt;

namespace Asp_Net_Good_idea.Helpers.UserHelpers
{
    public class UserServiceHelpers
    {
        private readonly AppDbContext _authContext;

        public UserServiceHelpers(AppDbContext authContext)
        {
            _authContext = authContext;
        }

        public async Task<bool> CheckUserTEIDAsync(string teid)
        {
            return await _authContext.Users.AnyAsync(x => x.TEID == teid);
        }

        public async Task<bool> CheckUserEMAILAsync(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                return await _authContext.Users.AnyAsync(x => x.Email == email);
            }
            return false;
        }

        public string CreateJwt(CreateJwt jwtData)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("YourSecureKeyHereWithAtLeast32BytesLong");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, jwtData.FulltName)
            };

            // Add role claim if user has a role
            if (!string.IsNullOrEmpty(jwtData.Role))
            {
                claims.Add(new Claim(ClaimTypes.Role, jwtData.Role));
            }

            // Add Id claim if user has an Id
            if (jwtData.Id != 0)
            {
                claims.Add(new Claim("Id", jwtData.Id.ToString()));
            }

            // Add Teid claim if user has a Teid
            if (!string.IsNullOrEmpty(jwtData.Teid))
            {
                claims.Add(new Claim("Teid", jwtData.Teid));
            }

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var expirationTime = DateTime.UtcNow.AddMinutes(1200);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expirationTime,
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        public string CreateRefreshToken()
        {
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var refreshToken = Convert.ToBase64String(tokenBytes);

            var tokenInUser = _authContext.Users
                .Any(a => a.RefreshToken == refreshToken);
            if (tokenInUser)
            {
                return CreateRefreshToken();
            }
            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipleFromExpiredToken(string token)
        {
            var key = Encoding.ASCII.GetBytes("YourSecureKeyHereWithAtLeast32BytesLong");
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("This is Invalid Token");
            return principal;
        }
    }
}
