using AutoMapper;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features
{
    public class EditSubjectModel : IRequest<EditSubjectResult>
    {
        public int Id { get; set; }
        public String SubjectName { get; set; }
    }

    public class EditSubjectResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class EditSubjectResultHandler : IRequestHandler<EditSubjectModel, EditSubjectResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IClientManagerService _clientManager;

        public EditSubjectResultHandler(InternalExamportalContext dbContext,
            IMapper mapper,
            IClientManagerService clientManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _clientManager = clientManager;
        }

        public async Task<EditSubjectResult> Handle(EditSubjectModel request, CancellationToken cancellationToken)
        {
            var result = new EditSubjectResult();

            var subject = _mapper.Map<Subject>(request);

            var oldSubject =await _dbContext.Subject.AsNoTracking()
                                                      .Where(prop => prop.Id == subject.Id)
                                                      .FirstOrDefaultAsync();

            subject.ClientId = oldSubject.ClientId;

            bool isExists = await _dbContext.Subject
                .AnyAsync(prop => prop.SubjectName == subject.SubjectName
                                && prop.ClientId == subject.ClientId
                                && prop.Id != subject.Id);

            if (isExists)
            {
                result.Success = false;
                result.Message = "Subject Already Exists";
                return result;
            }

            _dbContext.Subject.Update(subject);
            await _dbContext.SaveChangesAsync();
            result.Success = true;
            return result;
        }
    }

}
