using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Commons.Utils;
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
    public class UpdateActivationCodeModel : IRequest<UpdateActivationCodeResult>
    {
        public int Id { get; set; }
        public int ActivationTypeId { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public CommonCode CommonCode { get; set; }
        public AccessCode AccessCode { get; set; }
    }

    public class UpdateActivationCodeResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class UpdateActivationCodeHandler : IRequestHandler<UpdateActivationCodeModel, UpdateActivationCodeResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateActivationCodeHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UpdateActivationCodeResult> Handle(UpdateActivationCodeModel request, CancellationToken cancellationToken)
        {
            var result = new UpdateActivationCodeResult();

            ActivationCode activation = await _dbContext.ActivationCode
                                                    .Where(prop => prop.Id == request.Id)
                                                    .Include(prop => prop.CommonCode)
                                                    .Include(prop => prop.AccessCode)
                                                    .AsNoTracking()
                                                    .FirstOrDefaultAsync();

            var activationCode = _mapper.Map<ActivationCode>(request);

            if (request.ActivationTypeId == 1)
            {
                if (activation.CommonCode != null)
                {
                    activationCode.CommonCode.Id = activation.CommonCode.Id;
                }

                var codeExists = await _dbContext.CommonCode.AnyAsync(prop => prop.CommonTestCode == request.CommonCode.CommonTestCode);
                if (codeExists && request.CommonCode.CommonTestCode != activation.CommonCode.CommonTestCode)
                {
                    result.Success = false;
                    result.Message = "Test with same common code already exists.";
                    return result;
                }

                _dbContext.ActivationCode.Update(activationCode);
                await _dbContext.SaveChangesAsync();
                result.Success = true;
            }
            else if (request.ActivationTypeId == 2)
            {

                if (activation.AccessCode != null)
                {
                    activationCode.AccessCode.Id = activation.AccessCode.Id;
                }

                activationCode.AccessCode.AccessTestCode = Utilities.GetRandomString(10);

                _dbContext.ActivationCode.Update(activationCode);
                await _dbContext.SaveChangesAsync();
                result.Success = true;
            }


            return result;

        }
    }
}
