using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Commons.Utils;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin
{
    public class ActivationCodeModel : IRequest<ActivationCodeResult>
    {
        public int ActivationTypeId { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public CommonCode CommonCode { get; set; }
        public AccessCode AccessCode { get; set; }
    }

    public class ActivationCodeResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class ActivationCodeHandler : IRequestHandler<ActivationCodeModel, ActivationCodeResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public ActivationCodeHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<ActivationCodeResult> Handle(ActivationCodeModel request, CancellationToken cancellationToken)
        {
            var result = new ActivationCodeResult();

            if (request.ActivationTypeId == 2)
            {
                request.AccessCode.AccessTestCode = Utilities.GetRandomString(10);
            }
            else
            {
                var codeExists = await _dbContext.CommonCode.AnyAsync(prop => prop.CommonTestCode == request.CommonCode.CommonTestCode);
                if (codeExists)
                {
                    result.Success = false;
                    result.Message = "Test with same common code already exists.";
                    return result;
                }
            }

            var activationCode = _mapper.Map<ActivationCode>(request);

            await _dbContext.ActivationCode.AddAsync(activationCode);
            await _dbContext.SaveChangesAsync();
            result.Success = true;

            return result;
        }


    }
}
