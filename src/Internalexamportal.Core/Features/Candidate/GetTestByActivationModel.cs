using AutoMapper;
using Internalexamportal.Common.Extensions;
using Internalexamportal.Core.Commons.Utils;
using Internalexamportal.Core.Enums;
using Internalexamportal.Core.Features.Admin.testManagement;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Candidate
{
    public class GetTestByActivationModel : IRequest<GetTestByActivationModelResult>
    {
        public string ActivationTestCode { get; set; }
    }
    public class TestDetails
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public double Duration { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalMark { get; set; }
        public int TestTypeId { get; set; }
        public int TestInstructionId { get; set; }
        public int DifficultLevelId { get; set; }
        public Guid LinkId { get; set; }
    }

    public class GetTestByActivationModelResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public TestDetails TestDetails { get; set; }
        public List<SettingTypeResult> TestSettings { get; set; } = new List<SettingTypeResult>();
    }

    public class GetTestByActivationHandler : IRequestHandler<GetTestByActivationModel, GetTestByActivationModelResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPrincipal _principal;

        public GetTestByActivationHandler(InternalExamportalContext dbContext,
                                            IMapper mapper,
                                            IPrincipal principal)
        {
            _principal = principal;
            _mapper = mapper;
            _dbContext = dbContext;

        }

        public async Task<GetTestByActivationModelResult> Handle(GetTestByActivationModel request, CancellationToken cancellationToken)
        {
            var result = new GetTestByActivationModelResult();

            var userId = _principal.Identity.GetUserId();

            var settingTypes = _mapper.Map<List<SettingTypeResult>>(await _dbContext.TestSettingType.ToListAsync());

            var activation = _dbContext.ActivationCode
                        .Include(prop => prop.CommonCode)
                        .Include(prop => prop.AccessCode)
                        .Where(pro => ((pro.ActivationTypeId == 1) ?
                                        (pro.CommonCode.CommonTestCode == request.ActivationTestCode)
                                    : (pro.AccessCode.AccessTestCode == request.ActivationTestCode)))
                        .AsNoTracking()
                        .FirstOrDefault();

            if (activation != null && activation.ActivationTypeId == 1)
            {
                var testData = _dbContext.Test.Where(pro => pro.Id == activation.TestId)
                                                .Include(pro => pro.Setting)
                                                .Include(pro => pro.TestPublish)
                                                .AsNoTracking()
                                                .FirstOrDefault();

                bool isTestDatePassed = CheckTestPastDateValidation(testData.TestPublish);
                bool isTestDateScheduled = CheckTestFutureDateValidation(testData.TestPublish);

                if (isTestDatePassed && isTestDateScheduled)
                {
                    if (await CheckIfUserHasAppearedForTest(testData, userId))
                    {
                        result.TestDetails = _mapper.Map<TestDetails>(testData);
                        foreach (var item in settingTypes)
                        {
                            item.IsChecked = testData.Setting.Any(prop => prop.TestSettingTypeId == item.Id);
                            result.TestSettings.Add(item);
                        }
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "Multiple attempts are not allowed for this test.";
                    }
                }
                else
                {
                    if (!isTestDateScheduled)
                        result.Message = "Test not started yet.";
                    if (!isTestDatePassed)
                        result.Message = "Test has expired.";

                    result.Success = false;
                }
            }
            else if (activation != null && activation.ActivationTypeId == 2)
            {
                var userEmail = _dbContext.Users.Find(userId).Email;

                if (userEmail == activation.AccessCode.Email)
                {
                    var testData = _dbContext.Test.Where(pro => pro.Id == activation.TestId)
                                                    .Include(pro => pro.Setting)
                                                    .AsNoTracking()
                                                    .FirstOrDefault();

                    bool isTestDatePassed = CheckTestPastDateValidation(testData.TestPublish);
                    bool isTestDateScheduled = CheckTestFutureDateValidation(testData.TestPublish);

                    if (isTestDatePassed && isTestDateScheduled)
                    {
                        if (await CheckIfUserHasAppearedForTest(testData, userId))
                        {
                            result.TestDetails = _mapper.Map<TestDetails>(testData);
                            foreach (var item in settingTypes)
                            {
                                item.IsChecked = testData.Setting.Any(prop => prop.TestSettingTypeId == item.Id);
                                result.TestSettings.Add(item);
                            }
                            result.Success = true;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = "Multiple attempts are not allowed for this test.";
                        }
                    }
                    else
                    {
                        if (!isTestDateScheduled)
                            result.Message = "Test not started yet.";
                        if (!isTestDatePassed)
                            result.Message = "Test has expired.";

                        result.Success = false;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = "This test is not for this user";
                }
            }
            else
            {
                result.Success = false;
                result.Message = "This access code is invalid";
            }

            return result;
        }

        private bool CheckTestPastDateValidation(TestPublish testPublish)
        {
            if (testPublish.EndDate.HasValue)
            {
                var testEndDate = testPublish.EndDate.Value;

                var todaysDateTime = Utilities.GetISTDateTime();

                return (todaysDateTime < testEndDate);

            }
            return true;
        }

        private bool CheckTestFutureDateValidation(TestPublish testPublish)
        {
            if (testPublish.StartDate.HasValue)
            {
                var testStartDate = testPublish.StartDate.Value;

                var todaysDateTime = Utilities.GetISTDateTime();

                return (todaysDateTime > testStartDate);

            }

            return true;
        }

        private async Task<bool> CheckIfUserHasAppearedForTest(Test test, string userId)
        {
            var multipleAttemptSetting = test.Setting.Where(prop => prop.TestSettingTypeId == (int)TestSettingEnum.MultipleAttempts).FirstOrDefault();
            var appeared = await _dbContext.SubmittedTest.AnyAsync(prop => prop.TestId == test.Id && prop.UserId == userId);

            if ((multipleAttemptSetting != null && multipleAttemptSetting.IsSettingApplied) || !appeared)
            {
                return true;
            }
            return false;
        }
    }
}

