using AutoMapper;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin
{
    public class UpdateQuestionModel : IRequest<UpdateQuestionResult>
    {
        public Question Question { get; set; }
    }
    public class UpdateQuestionResult
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
    public class UpdateQuestionHandler : IRequestHandler<UpdateQuestionModel, UpdateQuestionResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IClientManagerService _clientManager;

        public UpdateQuestionHandler(InternalExamportalContext dbContext,
            IClientManagerService clientManager)
        {
            _dbContext = dbContext;
            _clientManager = clientManager;
        }

        public async Task<UpdateQuestionResult> Handle(UpdateQuestionModel request, CancellationToken cancellationToken)
        {
            //Regex rgx = new Regex("<p>|</p>");
           // request.Question.Description = rgx.Replace(request.Question.Description, "");

            var oldQues = await _dbContext.Question.AsNoTracking().Where(prop => prop.Id == request.Question.Id).FirstOrDefaultAsync();
            request.Question.ClientId = oldQues.ClientId;

            _dbContext.Question.Update(request.Question);
            await _dbContext.SaveChangesAsync();

            return new UpdateQuestionResult { success = true,message = "success"};
        }
    }
}


