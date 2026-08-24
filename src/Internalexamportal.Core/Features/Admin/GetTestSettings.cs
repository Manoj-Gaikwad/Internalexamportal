using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin
{
   public class GetTestSettingsRequestModel : IRequest<GetTestSettingResponseModel>
    {
        public int Id { get; set; }
    }
   public class GetTestSettingResponseModel
    {
        public int Id { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalMark { get; set; }
        public int QuestionTypeId { get; set; }
    }

    public class GetTestSettingsHandler : IRequestHandler<GetTestSettingsRequestModel,GetTestSettingResponseModel>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetTestSettingsHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public  Task<GetTestSettingResponseModel> Handle(GetTestSettingsRequestModel request, CancellationToken cancellationToken)
        {
            var testDetails = _dbContext.Test.Where(prop => prop.Id == request.Id).FirstOrDefault();
            return Task.FromResult(_mapper.Map<GetTestSettingResponseModel>(testDetails));
        }
    
    }
}
