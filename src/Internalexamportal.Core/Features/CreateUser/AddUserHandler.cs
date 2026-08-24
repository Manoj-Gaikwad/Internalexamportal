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

namespace Internalexamportal.Core.Features.AddUser
{

    //Model
    public class AddUserModel : IRequest<IdentityResult>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; } = true;
        public string Role { get; set; }
        public int ClientId { get; set; }
    }

    //Handler
    public class AddUserHandler : IRequestHandler<AddUserModel, IdentityResult>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IGenerateEmailConfirmationUrlService _generateEmailConfirmationUrl;
        private readonly IClientManagerService _clientManager;

        public AddUserHandler(
            IMapper mapper,
            UserManager<User> userManager,
            IEmailSenderService emailSenderService,
            IGenerateEmailConfirmationUrlService generateEmailConfirmationUrl,
            IClientManagerService clientManager
        )
        {
            _mapper = mapper;
            _userManager = userManager;
            _emailSenderService = emailSenderService;
            _generateEmailConfirmationUrl = generateEmailConfirmationUrl;
            _clientManager = clientManager;
        }

        public async Task<IdentityResult> Handle(AddUserModel message, CancellationToken token)
        {
            // map model to entity.
            message.UserName = message.Email;
            var user = _mapper.Map<User>(message);

            if(user.ClientId == 0)
            {
                var client = await _clientManager.GetClientId();
                user.ClientId = client.Item1;
            }

            user.RegistrationDate = Utilities.GetISTDateTime();
            //Generate Random Password
            message.Password = Utilities.GeneratePassword(8);

            // create user with the provided password.
            var identityResult = await _userManager.CreateAsync(user, message.Password);

            // if not success return identity result
            if (!identityResult.Succeeded) return identityResult;

            //Find the user created just now
            var createdUser = await _userManager.FindByNameAsync(user.UserName);

            // check if user has role
            if (!await _userManager.IsInRoleAsync(createdUser, message.Role))
            {
                //if not assign role
                var roleRsesult = await _userManager.AddToRoleAsync(createdUser, message.Role);

                if (!roleRsesult.Succeeded)
                {
                    throw new Exception("Could not assign candidate role to user.");
                }
            }

            try
            {
                //Call EmailSenderService to send confirmation email.
                await _emailSenderService.SendPasswordEmail(message.Email, message.Password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            // return identity result
            return identityResult;
        }
    }
}
