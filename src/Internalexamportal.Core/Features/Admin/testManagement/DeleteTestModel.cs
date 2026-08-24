using AutoMapper;
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
   public class DeleteTestModel : IRequest<DeleteTestResult>
    {
        public string Id { get; set; }
    }
    public class DeleteTestResult
    {

    }
    public class DeleteTestHandler : IRequestHandler<DeleteTestModel, DeleteTestResult>
    {
        private readonly IRepository<Test> _subject;
        private readonly IMapper _mapper;
        public DeleteTestHandler(IRepository<Test> subject, IMapper mapper)
        {
            _subject = subject;
            _mapper = mapper;

        }

        public async Task<DeleteTestResult> Handle(DeleteTestModel model, CancellationToken token)
        {
            DeleteTestResult result = new DeleteTestResult();
            var id = int.Parse(model.Id);
            var subjectId = _subject.Find(id);
            if (subjectId != null)
            {
                _subject.Remove(subjectId);
                _subject.SaveChanges();
            }
            return result;
        }


    }
}
