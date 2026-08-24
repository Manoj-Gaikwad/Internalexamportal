using AutoMapper;
using Internalexamportal.DataAccessLayer;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin
{
    public class GetTestSettingTypeModel : IRequest<ICollection<GetTestSettingTypeResult>>
    {
    }
    public class GetTestSettingTypeResult
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }

    public class GetTestSettingTypeHandler : IRequestHandler<GetTestSettingTypeModel, ICollection<GetTestSettingTypeResult>>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetTestSettingTypeHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Task<ICollection<GetTestSettingTypeResult>> Handle(GetTestSettingTypeModel request, CancellationToken cancellationToken)
        {
            var testSettingTypes = _dbContext.TestSettingType.ToList();
            return Task.FromResult(_mapper.Map<ICollection<GetTestSettingTypeResult>>(testSettingTypes));
        }


    }
}
