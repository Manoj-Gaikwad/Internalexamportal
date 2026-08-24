using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin.testManagement
{
    public class TestSettingModel : IRequest<TestSettingResult>
    {
        public int TestId { get; set; }
        public bool IsSettingApplied { get; set; }
        public bool MultipleAttemptsAllowed { get; set; }
    }
    public class TestSettingResult
    {
        public bool IsSettingApplied { get; set; }
        public int TestSettingTypeId { get; set; }
    }
    public class TestSettingHandler : IRequestHandler<TestSettingModel, TestSettingResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public TestSettingHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<TestSettingResult> Handle(TestSettingModel request, CancellationToken cancellationToken)
        {
            var result = new TestSettingResult();

            var shuffleSetting = new Setting
            {
                IsSettingApplied = request.IsSettingApplied,
                TestId = request.TestId,
                TestSettingTypeId = 1
            };
            await _dbContext.Setting.AddAsync(shuffleSetting);

            var multipleAttemptSetting = new Setting
            {
                IsSettingApplied = request.MultipleAttemptsAllowed,
                TestId = request.TestId,
                TestSettingTypeId = 3
            };
            await _dbContext.Setting.AddAsync(multipleAttemptSetting);

            _dbContext.SaveChanges();

            return result;
        }
    }
}
