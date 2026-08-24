using AutoMapper;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin.testManagement
{
    public class EmailResultToCandidateRequest : IRequest<EmailResultToCandidateResult>
    {
        public int Id { get; set; }
        public string WebsiteURL { get; set; }
    }
    public class EmailResultToCandidateResult
    {
        public bool Success { get; set; }
    }

    public class EmailResultToCandidateHandler : IRequestHandler<EmailResultToCandidateRequest, EmailResultToCandidateResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSenderService;

        public EmailResultToCandidateHandler(InternalExamportalContext dbContext,
            IMapper mapper,
            IEmailSenderService emailSenderService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _emailSenderService = emailSenderService;
        }

        public async Task<EmailResultToCandidateResult> Handle(EmailResultToCandidateRequest request, CancellationToken cancellationToken)
        {
            var result = new EmailResultToCandidateResult();

            var testResult = await _dbContext.SubmittedTest
                                    .Where(prop => prop.Id == request.Id)
                                    .Include(prop => prop.User)
                                    .Include(prop => prop.Test)
                                        .ThenInclude(prop => prop.Client)
                                    .OrderByDescending(prop => prop.SubmitDate)
                                    .Select(prop => new SendExamResultEmailModel
                                    {
                                        TestName = prop.Test.TestName,
                                        UserName = prop.User.FullName,
                                        SubmitDate = prop.SubmitDate.ToString("dddd, dd MMMM yyyy"),
                                        MaximumMark = prop.MaximumMark,
                                        TotalMark = prop.TotalMark,
                                        Email = prop.User.Email,
                                        ClientName = prop.Test.Client.Name,
                                        ClientEmail = prop.Test.Client.Email
                                    }).FirstOrDefaultAsync();

            try
            {
                //Call EmailSenderService to send email.
                await _emailSenderService.SendExamResult(testResult,request.WebsiteURL);
                result.Success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                result.Success = false;
            }


            return result;
        }

    }
}
