using Internalexamportal.Core.Configurations;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using Internalexamportal.Core.Commons;
using Newtonsoft.Json;

namespace Internalexamportal.Core.Features.GenerateJwtToken
{

    //Model
    public class GenerateJwtTokenModel : IRequest<GenerateTokenResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    //Result
    public class GenerateTokenResult
    {
        public bool IsValidCredential { get; set; }
        public string UserId { get; set; }
        public TokenResult TokenResult { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsActive { get; set; }
        public IList<string> Role { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; } = false;
    }

    public class TokenResult
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public string Permissions { get; set; }
    }

    //Handler
    public class GenerateJwtTokenHandler
        : IRequestHandler<GenerateJwtTokenModel, GenerateTokenResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenConfiguration _tokenConfiguration;
        private readonly IJwtSecurityTokenGenerationService _jwtSecurityTokenGenerationService;
        private readonly AuthenticationSettingsConfiguration _authSettings;
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly IGetRolePermissionService _getRolePermissionService;

        public GenerateJwtTokenHandler(
            UserManager<User> userManager,
            IOptions<TokenConfiguration> tokenConfiguration,
            IJwtSecurityTokenGenerationService jwtSecurityTokenGenerationService,
            IOptions<AuthenticationSettingsConfiguration> authSettings,
            IVerificationCodeService verificationCodeService,
            IGetRolePermissionService getRolePermissionService
        )
        {
            _userManager = userManager;
            _tokenConfiguration = tokenConfiguration.Value;
            _jwtSecurityTokenGenerationService = jwtSecurityTokenGenerationService;
            _authSettings = authSettings.Value;
            _verificationCodeService = verificationCodeService;
            _getRolePermissionService = getRolePermissionService;
        }

        public async Task<GenerateTokenResult> Handle(GenerateJwtTokenModel message, CancellationToken token)
        {
            var user = await _userManager.FindByEmailAsync(message.Email);

            var isValid = (user != null)
                && await _userManager.CheckPasswordAsync(user, message.Password)
                && !(await _userManager.IsLockedOutAsync(user));

            var result = new GenerateTokenResult() { IsValidCredential = isValid };

            if (!isValid)
            {
                return result;
            }

            result.UserId = user.Id;
            result.IsActive = user.IsActive;
            result.Role = await _userManager.GetRolesAsync(user);

            if (result.Role.Count > 0 && (result.Role[0].Equals(GlobalConstants.CandidateRoleName)))
            {
                result.IsAdmin = false;
            }
            else
            {
                result.IsAdmin = true;
            }

            if (isValid)
            {
                //Upon successfull login, reset the AccesFailedCount number for user in database. 
                if (_userManager.SupportsUserLockout && await _userManager.GetAccessFailedCountAsync(user) > 0)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MinValue);
                }

                //Configurable in appsetting.json.  If both project and users have 2FA turned on, 
                //then only send text message. 
                //if (_authSettings.TwoFactorEnabledForProject && user.TwoFactorEnabled)
                //{

                //   result.UserId = user.Id;

                //    //Get Verification Code
                //    string verificationCode = await _verificationCodeService.GenerateVerificationCode(user);

                //    //Send Verification Code
                //    await _smsNotificationService
                //        .SendAuthenticationCodeAsync(user.PhoneNumber, verificationCode);

                //    return result;
                //}

                //Generate JWT Token
                var jwtSecurityToken =
                    await _jwtSecurityTokenGenerationService.GetJwtSecurityToken(user, message.RememberMe, _tokenConfiguration);
                var permissions = await _getRolePermissionService.GetRolePermissions(user);

                result.TokenResult = new TokenResult()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Expiration = jwtSecurityToken.ValidTo,
                    Permissions = JsonConvert.SerializeObject(permissions)
                };
            }

            //Check if the user is Locked Out of the System
            result.IsLockedOut = (user != null) && await _userManager.IsLockedOutAsync(user);

            //Add to AccessFailedCount upon failed login for user in database. 
            if ((user != null) && (_userManager.SupportsUserLockout && await _userManager.GetLockoutEnabledAsync(user)))
            {
                //Double the lockout time on every login fail after the user gets locked out. 
                if (result.IsLockedOut)
                {
                    await _userManager.SetLockoutEndDateAsync(user, user.LockoutEnd.Value.AddMinutes(user.LockoutEnd.Value.Minute));
                }

                await _userManager.AccessFailedAsync(user);

            }

            return result;
        }
    }
}
