using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin
{
    public class EmailExamLinkModel : IRequest<EmailExamLinkResult>
    {
        public string Email { get; set; }
        public int TestId { get; set; }
        public string ExamUrl { get; set; }
        public int? Group { get; set; }
    }

    public class EmailExamLinkResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class EmailExamLinkHandler : IRequestHandler<EmailExamLinkModel, EmailExamLinkResult>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly InternalExamportalContext _dbContext;
        private readonly IEmailSenderService _emailSenderService;
        public EmailExamLinkHandler(IMapper mapper, IEmailSenderService emailSenderService, UserManager<User> userManager,
            InternalExamportalContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
            _emailSenderService = emailSenderService;

        }

        public async Task<EmailExamLinkResult> Handle(EmailExamLinkModel request, CancellationToken cancellationToken)
        {
            var result = new EmailExamLinkResult();
            User user = await _userManager.FindByEmailAsync(request.Email);


            var activation = _dbContext.ActivationCode
                        .Where(prop => prop.TestId == request.TestId)
                        .Include(prop => prop.AccessCode)
                        .Include(prop => prop.CommonCode)
                        .FirstOrDefault();

            string activationCode = string.Empty;

            if (activation == null)
            {
                result.Success = false;
                result.Message = "Please select and save activation type before sending email.";
                return result;
            }
            else if (activation.ActivationTypeId == 1)
            {
                activationCode = activation.CommonCode.CommonTestCode;
            }
            else if (activation.ActivationTypeId == 2)
            {
                if (activation.AccessCode.Email == request.Email)
                {
                    activationCode = activation.AccessCode.AccessTestCode;
                }
                else
                {
                    result.Success = false;
                    result.Message = "This test is not for the selected candidate.";
                    return result;
                }
            }

            if (string.IsNullOrEmpty(activationCode))
            {
                result.Success = false;
                result.Message = "Please select and save activation type before sending email.";
                return result;
            }

            if (request.Group.HasValue)
            {
                var emails = await GetEmailsOfSelectedGroup(request.Group.Value);

                foreach (var email in emails)
                {
                    result.Success = await SendEmail(email, activationCode, request.ExamUrl);
                }
            }
            else if (!string.IsNullOrEmpty(request.Email))
            {
                result.Success = await SendEmail(request.Email, activationCode, request.ExamUrl);
            }

            return result;
        }

        public async Task<List<string>> GetEmailsOfSelectedGroup(int groupId)
        {
            return await _dbContext.CandidateGroup
                     .Include(prop => prop.User)
                     .Where(prop => prop.GroupId == groupId)
                     .Where(prop => prop.User.IsActive)
                     .Select(prop => prop.User.Email)
                     .ToListAsync();
        }

        public async Task<bool> SendEmail(string email, string activationCode, string examUrl)
        {
            bool success;
            try
            {
                await _emailSenderService.SendExamLink(email, activationCode, examUrl);
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                success = false;

            }
            return success;
        }
    }
}
