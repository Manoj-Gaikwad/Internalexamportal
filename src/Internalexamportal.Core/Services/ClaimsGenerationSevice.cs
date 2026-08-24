using Internalexamportal.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Internalexamportal.Core.Services
{
    public class ClaimsGenerationService : IClaimsGenerationService
    {
        public IEnumerable<Claim> GetTokenClaims(User user)
        {
            return new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)

                //.. more claims you want to store in token goes here 


                //TODO add roles claims here 
            };
        }
    }
}
