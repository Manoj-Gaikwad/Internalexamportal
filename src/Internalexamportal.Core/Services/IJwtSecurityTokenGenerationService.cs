using Internalexamportal.Core.Configurations;
using Internalexamportal.DataAccessLayer.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Services
{
    public interface IJwtSecurityTokenGenerationService
    {
         Task<JwtSecurityToken> GetJwtSecurityToken(
            User user, bool setExpiry, TokenConfiguration configuration);

         //IEnumerable<Claim> GetTokenClaims(User user);
    }
}
