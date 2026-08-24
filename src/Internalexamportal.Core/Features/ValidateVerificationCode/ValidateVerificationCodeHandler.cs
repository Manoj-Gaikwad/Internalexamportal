using System.Threading.Tasks;
using Internalexamportal.Core.Features.GenerateJwtToken;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Internalexamportal.DataAccessLayer.Entities;
using Internalexamportal.Core.Services;
using Internalexamportal.Core.Configurations;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Threading;

namespace Internalexamportal.Core.Features.ValidateVerificationCode
{

    //Model
    public class ValidateVerificationCodeModel : IRequest<ValidateVerificationCodeResult>
    {
        public string UserId { get; set; }
        public string Code { get; set; }
        public bool RememberMe { get; set; }

    }

    //Result
    public class ValidateVerificationCodeResult
    {
        public TokenResult TokenResult { get; set; }
        public bool Succeeded { get; set; }
    }

    //Handler
    public class CreateUserHandler : IRequestHandler<ValidateVerificationCodeModel, ValidateVerificationCodeResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtSecurityTokenGenerationService _jwtSecurityTokenService;
        private readonly TokenConfiguration _tokenConfig;
        public CreateUserHandler(
            UserManager<User> userManager,
            IJwtSecurityTokenGenerationService jwtSecurityTokenService,
            IOptions<TokenConfiguration> tokenConfig
            )
        {
            _userManager = userManager;
            _jwtSecurityTokenService = jwtSecurityTokenService;
            _tokenConfig = tokenConfig.Value;
        }
        public async Task<ValidateVerificationCodeResult> Handle(ValidateVerificationCodeModel model, CancellationToken token)
        {
            var result = new ValidateVerificationCodeResult();

            var user = await _userManager.FindByIdAsync(model.UserId);

            //Check if the code matches. Only log users in if the the generated and incoming code match.
            if (await _userManager.VerifyTwoFactorTokenAsync(user, "Phone", model.Code))
            {
                var jwtSecurityToken = await _jwtSecurityTokenService.GetJwtSecurityToken(user, model.RememberMe, _tokenConfig);

                result.TokenResult = new TokenResult()
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                    Expiration = jwtSecurityToken.ValidTo
                };

                result.Succeeded = true;

            }

            return result;
        }
    }
}

