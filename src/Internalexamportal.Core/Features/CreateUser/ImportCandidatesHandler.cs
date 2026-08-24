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
using System.Collections.Generic;
using System.Linq;
using InternalExamportal.DataAccessLayer.Entities;
using Internalexamportal.DataAccessLayer;

namespace Internalexamportal.Core.Features.ImportCandidates
{

    //Model
    public class ImportCandidatesModel : IRequest<ImportCandidatesResult>
    {
        public List<ImportCandidateModel> Candidates { get; set; }
        public int GroupId { get; set; }
    }

    public class ImportCandidateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; } = true;
    }

    public class ImportCandidatesResult
    {
        public int SuccessCount { get; set; }
        public int TotalCount { get; set; }
    }

    //Handler
    public class ImportCandidatesHandler : IRequestHandler<ImportCandidatesModel, ImportCandidatesResult>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailSenderService _emailSenderService;
        private readonly IGenerateEmailConfirmationUrlService _generateEmailConfirmationUrl;
        private readonly InternalExamportalContext _dbContext;
        private readonly IClientManagerService _clientManager;
        private readonly IRollNumberingService _rollNumberingService;

        public ImportCandidatesHandler(
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
            _emailSenderService = emailSenderService;
            _generateEmailConfirmationUrl = generateEmailConfirmationUrl;
            _dbContext = dbContext;
            _clientManager = clientManager;
            _rollNumberingService = rollNumberingService;
        }

        public async Task<ImportCandidatesResult> Handle(ImportCandidatesModel message, CancellationToken token)
        {
            var result = new ImportCandidatesResult { SuccessCount = 0, TotalCount = message.Candidates.Count() };
            var client = await _clientManager.GetClientId();

            foreach (var item in message.Candidates)
            {
                if (await CreateUser(item, message.GroupId, client.Item1))
                {
                    result.SuccessCount++;
                }
            }

            return result;
        }

        private async Task<bool> CreateUser(ImportCandidateModel model, int groupId, int clientId)
        {
            // map model to entity.
            model.UserName = model.Email;
            var user = _mapper.Map<User>(model);
            user.RegistrationDate = Utilities.GetISTDateTime();
            user.ClientId = clientId;
            user.RollNumber = await _rollNumberingService.GetNextRollNumber();
            //Generate Random Password
            model.Password = Utilities.GeneratePassword(8);

            // create user with the provided password.
            var identityResult = await _userManager.CreateAsync(user, model.Password);

            // if not success return identity result
            if (!identityResult.Succeeded)
            {
                await _rollNumberingService.RevertRollNumberCount();
                return identityResult.Succeeded;
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
                    return roleRsesult.Succeeded;
                }
            }

            //Add to new groups
            _dbContext.CandidateGroup.Add(new CandidateGroup { GroupId = groupId, UserId = createdUser.Id });
            await _dbContext.SaveChangesAsync();

            try
            {
                //Call EmailSenderService to send confirmation email.
                await _emailSenderService.SendPasswordEmail(model.Email, model.Password);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            // return identity result
            return identityResult.Succeeded;
        }
    }
}
