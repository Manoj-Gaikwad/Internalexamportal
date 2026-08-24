using AutoMapper;
using Internalexamportal.Core.Enums;
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
    public class SendExamResultEmailRequest : IRequest<SendExamResultEmailResult>
    {
        public int TestId { get; set; }
        public string WebsiteURL { get; set; }
    }
    public class SendExamResultEmailResult
    {
        public bool Success { get; set; }
    }

    public class SendExamResultEmailModel
    {
        public string TestName { get; set; }
        public string SubmitDate { get; set; }
        public string UserName { get; set; }
        public int MaximumMark { get; set; }
        public int TotalMark { get; set; }
        public string Email { get; set; }
        public int StatusId { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
    }

    public class SendExamResultEmailHandler : IRequestHandler<SendExamResultEmailRequest, SendExamResultEmailResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IEmailSenderService _emailSenderService;

        public SendExamResultEmailHandler(InternalExamportalContext dbContext,
            IMapper mapper,
            IEmailSenderService emailSenderService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _emailSenderService = emailSenderService;
        }

        public async Task<SendExamResultEmailResult> Handle(SendExamResultEmailRequest request, CancellationToken cancellationToken)
        {
            var result = new SendExamResultEmailResult();

            var resultList = await _dbContext.SubmittedTest
                                    .Where(prop => prop.TestId == request.TestId)
                                    .Where(prop => prop.StatusId == (int)TestStatusEnum.Evaluated)
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
                                        StatusId = prop.StatusId,
                                        ClientName = prop.Test.Client.Name,
                                        ClientEmail = prop.Test.Client.Email
                                    }).ToListAsync();

            foreach (var model in resultList)
            {
                if (model.StatusId == (int)TestStatusEnum.Evaluated)
                {
                    await SendResultEmail(model, request.WebsiteURL);
                }
            }

            result.Success = true;

            return result;
        }

        private async Task<bool> SendResultEmail(SendExamResultEmailModel model, string url)
        {
            try
            {
                //Call EmailSenderService to send email.
                await _emailSenderService.SendExamResult(model, url);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }


    }
}
