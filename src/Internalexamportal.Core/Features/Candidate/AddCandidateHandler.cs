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
using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;

namespace Internalexamportal.Core.Features.Candidate
{

    //Model
    public class AddCandidateModel : IRequest<IdentityResult>
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
        public int ClientId { get; set; }
        public int[] Group { get; set; }

    }

    //Handler
    public class AddCandidateHandler : IRequestHandler<AddCandidateModel, IdentityResult>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly InternalExamportalContext _dbContext;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IGenerateEmailConfirmationUrlService _generateEmailConfirmationUrl;
        private readonly IClientManagerService _clientManager;
        private readonly IRollNumberingService _rollNumberingService;

        public AddCandidateHandler(
            IMapper mapper,
            UserManager<User> userManager,
            IEmailSenderService emailSenderService,
            IGenerateEmailConfirmationUrlService generateEmailConfirmationUrl,
            InternalExamportalContext dbContext,
            IClientManagerService clientManager,
            IRollNumberingService rollNumberingService
        )
        {
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
            _emailSenderService = emailSenderService;
            _generateEmailConfirmationUrl = generateEmailConfirmationUrl;
            _clientManager = clientManager;
            _rollNumberingService = rollNumberingService;
        }

        public async Task<IdentityResult> Handle(AddCandidateModel message, CancellationToken token)
        {
            // map model to entity.
            message.UserName = message.Email;
            var user = _mapper.Map<User>(message);
            user.RegistrationDate = Utilities.GetISTDateTime();
            user.RollNumber = await _rollNumberingService.GetNextRollNumber();

            if (user.ClientId == 0)
            {
                var client = await _clientManager.GetClientId();
                user.ClientId = client.Item1;
            }

            //Generate Random Password
            message.Password = Utilities.GeneratePassword(8);

            // create user with the provided password.
            var identityResult = await _userManager.CreateAsync(user, message.Password);

            // if not success return identity result
            if (!identityResult.Succeeded)
            {
                await _rollNumberingService.RevertRollNumberCount();
                return identityResult;
            };

            //Find the user created just now
            var createdUser = await _userManager.FindByNameAsync(user.UserName);


            // check if user has role
            if (!await _userManager.IsInRoleAsync(createdUser, GlobalConstants.CandidateRoleName))
            {
                //if not assign role
                var roleRsesult = await _userManager.AddToRoleAsync(createdUser, GlobalConstants.CandidateRoleName);

                if (!roleRsesult.Succeeded)
                {
                    throw new Exception("Could not assign candidate role to user.");
                }
            }


            //Add new groups
            foreach (int group in message.Group)
            {
                _dbContext.CandidateGroup.Add(new CandidateGroup { GroupId = group, UserId = createdUser.Id });
            }
            await _dbContext.SaveChangesAsync();

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
