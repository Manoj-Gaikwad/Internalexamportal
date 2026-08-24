using Internalexamportal.DataAccessLayer.Entities;
using System.Collections.Generic;
using System.Security.Claims;

namespace Internalexamportal.Core.Services
{
    public interface IClaimsGenerationService
    {
        IEnumerable<Claim> GetTokenClaims(User user);
    }
}
