using AutoMapper;
using Internalexamportal.Core.Commons.Utils;
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
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin.testManagement
{
    public class UpdateTestDetailsModel : IRequest<UpdateTestDetailsResult>
    {
        public TestResultModel TestDetails { get; set; }
        public TestPublish TestPublish { get; set; }
        public ActivationCode TestActivation { get; set; }
        public List<SettingTypeResult> TestSettings { get; set; }
    }
    public class UpdateTestDetailsResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int TestId { get; set; }
        public int PublishId { get; set; }
        public int ActivationId { get; set; }
        public string Code { get; set; }
    }
    public class UpdateTestDetailsHandler : IRequestHandler<UpdateTestDetailsModel, UpdateTestDetailsResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IClientManagerService _clientManager;

        public UpdateTestDetailsHandler(InternalExamportalContext dbContext,
            IMapper mapper,
            IClientManagerService clientManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _clientManager = clientManager;
        }

        public async Task<UpdateTestDetailsResult> Handle(UpdateTestDetailsModel request, CancellationToken cancellationToken)
        {
            var result = new UpdateTestDetailsResult();
            var client = await _clientManager.GetClientId();

            var test = _mapper.Map<Test>(request.TestDetails);
            test.ClientId = client.Item1;
            test.TestPublish = request.TestPublish;

            if (test.Id == 0)
            {
                await _dbContext.AddAsync(test);
            }
            else
            {
                var oldtest = await _dbContext.Test.AsNoTracking().Where(prop => prop.Id == test.Id)
                                                          .FirstOrDefaultAsync();
                test.ClientId = oldtest.ClientId;
                _dbContext.Update(test);
            }

            await _dbContext.SaveChangesAsync();

            //Remove Existing Test Settings
            var testSettings = _dbContext.Setting.Where(v => v.TestId == test.Id);
            _dbContext.Setting.RemoveRange(testSettings);
            await _dbContext.SaveChangesAsync();

            //Add Test Settings
            foreach (var item in request.TestSettings)
            {
                if (item.IsChecked)
                {
                    var setting = new Setting
                    {
                        Id = 0,
                        TestSettingTypeId = item.Id,
                        TestId = test.Id,
                        IsSettingApplied = true
                    };

                    _dbContext.Setting.Add(setting);
                }
            }
            await _dbContext.SaveChangesAsync();


            result.TestId = test.Id;
            result.PublishId = test.TestPublish.Id;

            ActivationCode activation;
            if (request.TestDetails.Id == 0)
            {
                request.TestActivation.TestId = test.Id;
                if (request.TestActivation.ActivationTypeId == 2)
                {
                    request.TestActivation.AccessCode.AccessTestCode = Utilities.GetRandomString(10);
                }
                else
                {
                    result.Code = Utilities.GetRandomString(10);
                    request.TestActivation.CommonCode.CommonTestCode = result.Code;
                }
                activation = request.TestActivation;
                await _dbContext.ActivationCode.AddAsync(activation);
                await _dbContext.SaveChangesAsync();
                result.ActivationId = activation.Id;
            }
            else
            {

                if (request.TestActivation.ActivationTypeId == 1)
                {
                    var codeExists = await _dbContext.CommonCode
                                .AnyAsync(prop => prop.CommonTestCode == request.TestActivation.CommonCode.CommonTestCode
                                                && prop.ActivationCodeId != request.TestActivation.Id);
                    if (codeExists)
                    {
                        result.ActivationId = request.TestActivation.Id;
                        result.Success = false;
                        result.Message = "Test with same common code already exists.";
                        return result;
                    }
                }

                activation = await _dbContext.ActivationCode
                                     .Include(prop => prop.AccessCode)
                                     .Include(prop => prop.CommonCode)
                                     .Where(prop => prop.Id == request.TestActivation.Id)
                                     .AsNoTracking()
                                     .FirstOrDefaultAsync();

                if (request.TestActivation.ActivationTypeId == 1)
                {
                    activation.ActivationTypeId = 1;
                    if (activation.CommonCode != null)
                        request.TestActivation.CommonCode.Id = activation.CommonCode.Id;
                    activation.CommonCode = request.TestActivation.CommonCode;
                    activation.CommonCode.CommonTestCode = request.TestActivation.CommonCode.CommonTestCode;
                }
                if (request.TestActivation.ActivationTypeId == 2)
                {
                    activation.ActivationTypeId = 2;
                    if (activation.AccessCode != null)
                        request.TestActivation.AccessCode.Id = activation.AccessCode.Id;
                    activation.AccessCode = request.TestActivation.AccessCode;
                    activation.AccessCode.AccessTestCode = Utilities.GetRandomString(10);
                    activation.AccessCode.Email = request.TestActivation.AccessCode.Email;
                }
                result.ActivationId = activation.Id;
                _dbContext.ActivationCode.Update(activation);
                await _dbContext.SaveChangesAsync();
            }

            result.Success = true;
            return result;
        }
    }
}
