using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
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
    public class GetTestSettingTypesModel : IRequest<GetTestSettingTypesResult>
    {
        public int TestId { get; set; }
    }

    public class GetTestSettingTypesResult
    {
        public List<SettingTypeResult> TestSettings { get; set; }
    }

    public class SettingTypeResult
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool IsChecked { get; set; }
    }
    public class GetTestSettingTypesHandler : IRequestHandler<GetTestSettingTypesModel, GetTestSettingTypesResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public GetTestSettingTypesHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<GetTestSettingTypesResult> Handle(GetTestSettingTypesModel request, CancellationToken cancellationToken)
        {
            var result = new GetTestSettingTypesResult
            {
                TestSettings = new List<SettingTypeResult>()
            };

            var settingTypes = _mapper.Map<List<SettingTypeResult>>(await _dbContext.TestSettingType.ToListAsync());

            var testSettings = _dbContext.Setting.Where(x => x.TestId == request.TestId).ToList();

            foreach (var item in settingTypes)
            {
                item.IsChecked = testSettings.Any(prop => prop.TestSettingTypeId == item.Id);
                result.TestSettings.Add(item);
            }

            return result;
        }
    }
}
