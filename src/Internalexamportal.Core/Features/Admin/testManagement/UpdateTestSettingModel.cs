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

namespace Internalexamportal.Core.Features.Admin.testManagement
{
  public  class UpdateTestSettingModel :IRequest<UpdateTestSettingResult>
    {
        public int TestId { get; set; }
        public bool IsSettingApplied { get; set; }
        public bool MultipleAttemptsAllowed { get; set; }
    }
    public class UpdateTestSettingResult
    {
        public bool Success { get; set; }
    }
    public class UpdateTestSettingHandler : IRequestHandler<UpdateTestSettingModel, UpdateTestSettingResult>
    {
        private readonly IRepository<Setting> _setting;
        private readonly IRepository<TestSetting> _testSetting;
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateTestSettingHandler(IRepository<Setting> setting,
            IRepository<TestSetting> testSetting,
            IMapper mapper,
            InternalExamportalContext dbContext)
        {
            _setting = setting;
            _testSetting = testSetting;
            _mapper = mapper;
            _dbContext = dbContext;
        }
        public async Task<UpdateTestSettingResult> Handle(UpdateTestSettingModel request, CancellationToken cancellationToken)
        {
            var result = new UpdateTestSettingResult();

            var shuffleSetting = _dbContext.Setting
                                        .Where(prop => prop.TestId == request.TestId && prop.TestSettingTypeId == 1)
                                        .FirstOrDefault();
            if (shuffleSetting != null)
            {
                shuffleSetting.IsSettingApplied = request.IsSettingApplied;
                _dbContext.Setting.Update(shuffleSetting);
            }
            else
            {
                shuffleSetting = new Setting
                {
                    IsSettingApplied = request.IsSettingApplied,
                    TestId = request.TestId,
                    TestSettingTypeId = 1
                };
                await _dbContext.Setting.AddAsync(shuffleSetting);
            }

            var multipleSetting = _dbContext.Setting
                                            .Where(prop => prop.TestId == request.TestId && prop.TestSettingTypeId == 3)
                                            .FirstOrDefault();

            if (multipleSetting != null)
            {
                multipleSetting.IsSettingApplied = request.MultipleAttemptsAllowed;
                _dbContext.Setting.Update(multipleSetting);
            }
            else
            {
                multipleSetting = new Setting
                {
                    IsSettingApplied = request.MultipleAttemptsAllowed,
                    TestId = request.TestId,
                    TestSettingTypeId = 3
                };
                await _dbContext.Setting.AddAsync(multipleSetting);
            }
            _dbContext.SaveChanges();

            result.Success = true;

            return result;
        }
    }
}
