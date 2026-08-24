using AutoMapper;
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
    public class EditTestModel : IRequest<ICollection<EditTestResult>>
    {
        public int Id { get; set; }
    }
    public class EditTestResult
    {

        public int Id { get; set; }
        public string TestName { get; set; }
        public double Duration { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalMark { get; set; }
        public int QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }
        public int TestInstructionId { get; set; }
        public TestInstruction TestInstruction { get; set; }
        public int DifficultLevelId { get; set; }
        public DifficultLevel DifficultLevel { get; set; }
        public Guid LinkId { get; set; }
        public ICollection<ActivationCode> ActivationCode { get; set; }
        public TestPublish TestPublish { get; set; }
        public ICollection<TestQuestion> TestQuestion { get; set; }
        public ICollection<Setting> Setting { get; set; }



    }
    public class EditTestResulttHandler : IRequestHandler<EditTestModel,ICollection<EditTestResult>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Test> _test;
        public EditTestResulttHandler(IMapper mapper,
           IRepository<Test> test
           )
        {
            _mapper = mapper;
            _test = test;
           

        }
        public  Task<ICollection<EditTestResult>> Handle(EditTestModel request, CancellationToken cancellationToken)
        {
            var result = new EditTestResult();

            var test = _test.FindAll().Where(a => a.Id == request.Id).Include(a => a.TestPublish)
                .Include(a => a.TestQuestion).Include(a => a.Setting)
                .Include(a => a.ActivationCode).ThenInclude(a => a.AccessCode)
                .Include(a => a.ActivationCode).ThenInclude(a => a.CommonCode).ToList();
            return Task.FromResult(_mapper.Map<ICollection<EditTestResult>>(test));
        }
    }
}
