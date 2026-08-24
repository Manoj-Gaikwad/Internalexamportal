using AutoMapper;
using Internalexamportal.Common.Extensions;
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
    public class GetTestByTestId : IRequest<GetTestResult>
    {
        public int Id { get; set; }
    }
    public class GetTestResult
    {
            public int Id { get; set; }
            public string TestName { get; set; }
            public double Duration { get; set; }
            public int TotalQuestion { get; set; }
            public int TotalMark { get; set; }
            public int QuestionTypeId { get; set; }
            public int TestInstructionId { get; set; }
            public int DifficultLevelId { get; set; }
            public ICollection<TestQuestion>QuestionData { get; set; }
            public Guid LinkId { get; set; }

    }

    public class GetTestByTestIdHandler : IRequestHandler<GetTestByTestId, GetTestResult>
    {
        private readonly IRepository<Test> _test; 
        private readonly IRepository<TestQuestion> _testQuestion;
        private readonly IRepository<Setting> _setting;
        private readonly IRepository<TestSetting> _testSetting;
        private readonly IMapper _mapper;
        public GetTestByTestIdHandler(IRepository<Test> test, IMapper mapper
            , IRepository<TestQuestion> testQuestion, IRepository<TestSetting> testSetting,
            IRepository<Setting> setting)
        {
            _test = test;
            _testQuestion = testQuestion;
            _setting = setting;
            _testSetting = testSetting;
            _mapper = mapper;
        }


        public async Task<GetTestResult> Handle(GetTestByTestId request, CancellationToken cancellationToken)
        {
            var result = new GetTestResult();
            var testData = _test.Find(request.Id);
            var questionOptionData = _testQuestion
                .FindAll()
                .Where(prop => prop.TestId == testData.Id).ToList();
            var setting = _setting.FindAll().Where(pro => pro.TestId == testData.Id);
         foreach(var item in setting)
            {
                if (item.IsSettingApplied == true)
                {
                    
                    List<TestQuestion> orders = new List<TestQuestion>();
                    foreach (var orderModel in CollectionExtension.Randomize(questionOptionData))
                    {
                     
                        var data = orderModel;
                        orders.Add(data);
                    }
                    result.QuestionData = orders;
                }
            }
            
                return result;
        }
    }
}
