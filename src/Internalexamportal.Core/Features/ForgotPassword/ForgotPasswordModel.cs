using AutoMapper;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.ForgotPassword
{
    public class ForgotPasswordModel : IRequest<ForgotPasswordResult>
    {
        public string Email { get; set; }
        public string BaseUrl { get; set; }
    }

    public class ForgotPasswordResult
    {
        public bool Succeeded { get; set; }
    }

    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordModel, ForgotPasswordResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IForgotPasswordLinkUrlService _forgotPasswordLinkUrlService;
        private readonly ISmsNotificationService _smsNotificationService;
        private readonly IMapper _mapper;

        public ForgotPasswordHandler(
            UserManager<User> userManager,
            IEmailSenderService emailSenderService,
            IForgotPasswordLinkUrlService forgotPasswordLinkUrl,
            ISmsNotificationService smsNotificationService,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _emailSenderService = emailSenderService;
            _forgotPasswordLinkUrlService = forgotPasswordLinkUrl;
            _smsNotificationService = smsNotificationService;
            _mapper = mapper;
        }
        public async Task<ForgotPasswordResult> Handle(ForgotPasswordModel model, CancellationToken cancellationToken)
        {
            var result = new ForgotPasswordResult();

            User user = await _userManager.FindByEmailAsync(model.Email);
            //throw new NotImplementedException();

            if (user != null)
            {
                //Generate code for email confirmation using usermanager
                string code = await _userManager.GeneratePasswordResetTokenAsync(user);

                //encode the generated code to preserve characters while re-routig
                byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(code);

                string resetPasswordCode = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);

                //Call GenerateEmailConfirmationUrlService to generate url 
                //to be used as call back url in confirmation email.

                string callbackUrl = _forgotPasswordLinkUrlService.GetForgotPasswordUrl(
                  user.Id, resetPasswordCode, model.BaseUrl);

                //if (user.PhoneNumber != null)
                //{
                //    //Call sms service to send forgot password link.
                //    await _smsNotificationService.SendForgotPasswordLink(user.PhoneNumber, callbackUrl);
                //}

                //Call EmailSenderService to send forgot password link.
                await _emailSenderService.SendForgotPasswordLink(model.Email, callbackUrl);

                result.Succeeded = true;
            }
            else
            {
                result.Succeeded = false;
            }
            

            return  result;
        }
    }

}
