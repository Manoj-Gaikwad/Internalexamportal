using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.ClientFeatures
{
    public class GetClientByIdModel : IRequest<GetClientByIdResult>
    {
        public int Id { get; set; }
    }
    public class GetClientByIdResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
    }

    public class GetClientByIdHandler : IRequestHandler<GetClientByIdModel, GetClientByIdResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public GetClientByIdHandler(IMapper mapper, InternalExamportalContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<GetClientByIdResult> Handle(GetClientByIdModel request, CancellationToken cancellationToken)
        {

           var client = await _dbContext.Client.FindAsync(request.Id);

            var result = _mapper.Map<GetClientByIdResult>(client);

            return result;
        }
    }

}
