using Internalexamportal.Core.Configurations;
using Internalexamportal.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Services
{
    public class JwtSecurityTokenGenerationService : IJwtSecurityTokenGenerationService
    {
        private UserManager<User> _userManager;
        private IClaimsGenerationService _claimsGenerationService;
        public JwtSecurityTokenGenerationService
            (
            UserManager<User> userManager,
            IClaimsGenerationService claimsGenerationService
            )
        {
            _userManager = userManager;
            _claimsGenerationService = claimsGenerationService;
        }
        public async Task<JwtSecurityToken> GetJwtSecurityToken(
            User user, bool setExpiry, TokenConfiguration configuration)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {

                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            userClaims.Add(new Claim(ClaimTypes.Name, user.FullName));

            DateTime? expiryDateTime = !setExpiry
                ? DateTime.UtcNow.AddMinutes(configuration.DefaultExpirationInMinutes)
                : DateTime.UtcNow.AddDays(7);
             

            return new JwtSecurityToken(
                issuer: configuration.SiteUrl,
                audience: configuration.SiteUrl,
                claims: _claimsGenerationService.GetTokenClaims(user).Union(userClaims),
                expires: expiryDateTime,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.Key))
                    , SecurityAlgorithms.HmacSha256)
            );
        }
    }
}
