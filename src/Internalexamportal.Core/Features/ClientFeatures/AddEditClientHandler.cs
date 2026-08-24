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
    public class AddEditClientModel : IRequest<AddEditClientResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Logo { get; set; }
    }
    public class AddEditClientResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class AddEditClientHandler : IRequestHandler<AddEditClientModel, AddEditClientResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public AddEditClientHandler(IMapper mapper, InternalExamportalContext dbContext)
        {
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<AddEditClientResult> Handle(AddEditClientModel request, CancellationToken cancellationToken)
        {
            var result = new AddEditClientResult();

            var client = _mapper.Map<Client>(request);

            if (client.Id == 0)
            {
                bool isExists = await _dbContext.Client
                                                .AnyAsync(prop => prop.Name == client.Name
                                                                || prop.Email == client.Email
                                                                || prop.Phone == client.Phone);

                if (isExists)
                {
                    result.Success = false;
                    result.Message = "Client with similar data already exists";
                    return result;
                }
                else
                {
                    await _dbContext.Client.AddAsync(client);
                }
            }
            else
            {
                bool isExists = await _dbContext.Client.AnyAsync(prop => (prop.Name == client.Name
                                                                || prop.Email == client.Email
                                                                || prop.Phone == client.Phone)
                                                                && prop.Id != client.Id);

                if (isExists)
                {
                    result.Success = false;
                    result.Message = "Client with similar data already exists";
                    return result;
                }
                else
                {
                    _dbContext.Client.Update(client);
                }
            }

            await _dbContext.SaveChangesAsync();
            result.Success = true;
            return result;
        }
    }

}
