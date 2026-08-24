using Internalexamportal.Core.Configurations;
using Internalexamportal.Core.Features.GenerateJwtToken;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.ResetPassword
{
    public class ResetPasswordModel : IRequest<GenerateTokenResult>
    {
        public string UserId { get; set; }
        public string ForgotPasswordCode { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordHandler : IRequestHandler<ResetPasswordModel, GenerateTokenResult>
    {
        private readonly UserManager<User> _userManager;
       private readonly IPasswordChangeLogService _passwordChangeLogService;

        public ResetPasswordHandler(
            UserManager<User> userManager,
            IOptions<TokenConfiguration> tokenConfiguration,
            IPasswordChangeLogService passwordChangeLogService
        )
        {
            _userManager = userManager;
            _passwordChangeLogService = passwordChangeLogService;
        }

        public async Task<GenerateTokenResult> Handle(ResetPasswordModel model, CancellationToken cancellationToken)
        {
            User user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null) return new GenerateTokenResult();

            byte[] codeDecodedBytes = WebEncoders.Base64UrlDecode(model.ForgotPasswordCode);

            string codeDecoded = Encoding.UTF8.GetString(codeDecodedBytes);

            IdentityResult identityResult =
                await _userManager.ResetPasswordAsync(user, codeDecoded, model.NewPassword);

            //If the code matches, automatically log the user in. 
            if (!identityResult.Succeeded) return new GenerateTokenResult();

            //Instantiate object
            var result = new GenerateTokenResult
            {
                IsValidCredential = true,
                Username = user.UserName
               // UserName = user.UserName
            };

            //_passwordChangeLogService.SaveUpdatedPasswordChangeHistory(user.Id);

            return result;
        }
    }
}
