using AutoMapper;
using Internalexamportal.Common.Extensions;
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
    public class GetTestDetailsModel : IRequest<GetTestDetailsResult>
    {
        public int TestId { get; set; }
    }
    public class GetTestDetailsResult
    {
        public TestResultModel TestDetails { get; set; }
        public TestPublish TestPublish { get; set; }
        public ActivationCode TestActivation { get; set; }
        public List<SettingTypeResult> TestSettings { get; set; }

    }

    public class TestResultModel
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public double Duration { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalMark { get; set; }
        public int TestTypeId { get; set; }
        public int TestInstructionId { get; set; }
        public int DifficultLevelId { get; set; }
        public float? Percentage { get; set; }

    }

    public class GetTestDetailsHandler : IRequestHandler<GetTestDetailsModel, GetTestDetailsResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetTestDetailsHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public async Task<GetTestDetailsResult> Handle(GetTestDetailsModel request, CancellationToken cancellationToken)
        {
            var test = await _dbContext.Test
                                .Include(prop => prop.TestPublish)
                                .Include(prop => prop.Setting)
                                .Include(prop => prop.ActivationCode)
                                    .ThenInclude(prop => prop.CommonCode)
                                .Include(prop => prop.ActivationCode)
                                    .ThenInclude(prop => prop.AccessCode)
                                .Where(prop => prop.Id == request.TestId)
                                .FirstOrDefaultAsync();

            var result = _mapper.Map<GetTestDetailsResult>(test);

            result.TestSettings = new List<SettingTypeResult>();

            var settingTypes = _mapper.Map<List<SettingTypeResult>>(await _dbContext.TestSettingType.ToListAsync());

            foreach (var item in settingTypes)
            {
                item.IsChecked = test.Setting.Any(prop => prop.TestSettingTypeId == item.Id);
                result.TestSettings.Add(item);
            }

            result.TestActivation = test.ActivationCode.FirstOrDefault();

            return result;
        }
    }
}
