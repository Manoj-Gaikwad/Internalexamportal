using AutoMapper;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Org.BouncyCastle.Ocsp;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features
{
    public class AddQuestionRequestModel : IRequest<AddQuestionResultModel>
    {
        public Question Question { get; set; }
    }
    public class AddQuestionResultModel
    {
        public bool success { get; set; }
        public string message { get; set; }
    }
    public class CreateQuestionHandler : IRequestHandler<AddQuestionRequestModel, AddQuestionResultModel>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IClientManagerService _clientManager;

        public CreateQuestionHandler(InternalExamportalContext dbContext,
            IClientManagerService clientManager)
        {
            _dbContext = dbContext;
            _clientManager = clientManager;
        }
        public async Task<AddQuestionResultModel> Handle(AddQuestionRequestModel request, CancellationToken cancellationToken)
        {
            var client = await _clientManager.GetClientId();

            //Regex rgx = new Regex("<p>|</p>");
                     
            //request.Question.Description = rgx.Replace(request.Question.Description, "");
            request.Question.ClientId = client.Item1;

            await _dbContext.Question.AddAsync(request.Question);
            var return_val = await _dbContext.SaveChangesAsync();

            return new AddQuestionResultModel { success = (return_val > 0) , message = "Question Added Successfully" };
        }

    }
}