using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features
{
    public class SubjectModel : IRequest<GenerateSubjectResult>
    {
        public int Id { get; set; }
        public string SubjectName { get; set; }
    }
    public class GenerateSubjectResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

    }

    public class CreateSubjectHandler : IRequestHandler<SubjectModel, GenerateSubjectResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IClientManagerService _clientManager;

        public CreateSubjectHandler(InternalExamportalContext dbContext,
            IMapper mapper,
            IClientManagerService clientManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _clientManager = clientManager;
        }
        public async Task<GenerateSubjectResult> Handle(SubjectModel request, CancellationToken cancellationToken)
        {
            var result = new GenerateSubjectResult();

            var subject = _mapper.Map<Subject>(request);

            var client = await _clientManager.GetClientId();
            subject.ClientId = client.Item1;

            bool isExists = await _dbContext.Subject
                            .AnyAsync(prop => prop.SubjectName == subject.SubjectName
                                            && prop.ClientId == subject.ClientId);

            if (isExists)
            {
                result.Success = false;
                result.Message = "Subject Already Exists";
                return result;
            }

            await _dbContext.Subject.AddAsync(subject);
            await _dbContext.SaveChangesAsync();
            result.Success = true;

            return result;
        }
    }
}