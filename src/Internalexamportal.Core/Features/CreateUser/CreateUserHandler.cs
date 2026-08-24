using AutoMapper;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Threading;
using System;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Commons.Utils;

namespace Internalexamportal.Core.Features.CreateUser
{

    //Model
    public class CreateUserModel : IRequest<IdentityResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool AgreedToTerms { get; set; }
        public string PhoneNumber { get; set; }
        public int ClientId { get; set; } = 1;
        
    }

    //Handler
    public class CreateUserHandler : IRequestHandler<CreateUserModel, IdentityResult>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IGenerateEmailConfirmationUrlService _generateEmailConfirmationUrl;
        private readonly IRollNumberingService _rollNumberingService;

        public CreateUserHandler(
            IMapper mapper,
            UserManager<User> userManager,
            IEmailSenderService emailSenderService,
            IGenerateEmailConfirmationUrlService generateEmailConfirmationUrl,
            IRollNumberingService rollNumberingService
        )
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailSenderService = emailSenderService;
            _generateEmailConfirmationUrl = generateEmailConfirmationUrl;
            _rollNumberingService = rollNumberingService;
        }

        public async Task<IdentityResult> Handle(CreateUserModel message, CancellationToken token)
        {
            // map model to entity.
            var user = _mapper.Map<User>(message);
            user.UserName = user.Email;
            user.RegistrationDate = Utilities.GetISTDateTime();
            user.RollNumber = await _rollNumberingService.GetNextRollNumber();

            // create user with the provided password.
            var identityResult = await _userManager.CreateAsync(user, message.Password);

            // if not success return identity result
            if (!identityResult.Succeeded)
            {
                await _rollNumberingService.RevertRollNumberCount();
                return identityResult;
            };

            //Generate code for email confirmation using usermanager
            // var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            //Find the user created just now
            var createdUser = await _userManager.FindByNameAsync(message.UserName);

            //Call GenerateEmailConfirmationUrlService to generate url 
            //to be used as call back url in confirmation email.
            //var callbackUrl = _generateEmailConfirmationUrl.GetEmailConfirmationUrl(
            //    createdUser.Id, emailConfirmationCode);

            //Call EmailSenderService to send confirmation email.
            // _emailSenderService.SendConfirmationEmail(message.Email, callbackUrl);

            // check if user has admin role
            if (!await _userManager.IsInRoleAsync(createdUser, GlobalConstants.CandidateRoleName))
            {
                //if not assign admin role
                var roleRsesult = await _userManager.AddToRoleAsync(createdUser, GlobalConstants.CandidateRoleName);

                if (!roleRsesult.Succeeded)
                {
                    throw new Exception("Could not assign admin role to default user.");
                }
            }

            // return identity result
            return identityResult;
        }
    }
}
