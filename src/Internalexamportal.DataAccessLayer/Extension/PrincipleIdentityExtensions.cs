using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Extension
{
    public static class PrincipleIdentityExtensions
    {
        /// <summary>
        /// Get claims value from JWT token for provided identity.
        /// values are assigned at <see cref="Core.Features.GenerateJwtTokenHandler"/>
        /// </summary>
        /// <param name="identity">User Identity</param>
        /// <param name="claimType">Claim Type</param>
        /// <returns>claim value</returns>
        public static string GetClaimsValue(this IIdentity identity, string claimType)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(claimType);
            return claim?.Value ?? string.Empty;
        }

        /// <summary>
        /// Gets <see cref="JwtRegisteredClaimNames.Sid"/> claims value from JWT token
        /// for provided identity
        /// </summary>
        /// <param name="identity">User Identity</param>
        /// <returns>claim value</returns>
        public static string GetUserId(this IIdentity identity)
        {
            return identity.GetClaimsValue(JwtRegisteredClaimNames.Sid);
        }

        /// <summary>
        /// Gets <see cref="ClaimTypes.NameIdentifier"/> claims value from JWT token
        /// </summary>
        /// <param name="identity">User Identity</param>
        /// <returns>claim value</returns>
        public static string GetUserName(this IIdentity identity)
        {
            // TODO "JwtRegisteredClaimNames.Sub" is not working find out why
            return identity.GetClaimsValue(ClaimTypes.NameIdentifier);
        }
    }
}
