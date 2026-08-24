using AutoMapper;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Threading;
using System;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Commons.Utils;
using Internalexamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using System.Linq;

namespace Internalexamportal.Core.Features.Candidate
{

    //Model
    public class AddGroupModel : IRequest<AddGroupResult>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AddGroupResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    //Handler
    public class AddGroupHandler : IRequestHandler<AddGroupModel, AddGroupResult>
    {
        private readonly IMapper _mapper;
        private readonly InternalExamportalContext _dbContext;
        private readonly IClientManagerService _clientManager;

        public AddGroupHandler(
            IMapper mapper,
            IClientManagerService clientManager,
            InternalExamportalContext dbContext
        )
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _clientManager = clientManager;
        }

        public async Task<AddGroupResult> Handle(AddGroupModel request, CancellationToken token)
        {
            var result = new AddGroupResult();

            var client = await _clientManager.GetClientId();

            if (request.Id == 0)
            {
                bool isExists = await _dbContext.Group.AnyAsync(prop => prop.Name == request.Name
                                                        && prop.ClientId == client.Item1);
                if (isExists)
                {
                    result.Success = false;
                    result.Message = "Group with same name already exists";
                    return result;
                }
                await _dbContext.Group.AddAsync(new Group { Name = request.Name,ClientId = client.Item1 });
            }
            else
            {
                var group = await _dbContext.Group.FindAsync(request.Id);

                bool isExists = await _dbContext.Group
                                        .AnyAsync(prop => prop.Name == request.Name
                                                        && prop.ClientId == group.ClientId
                                                        && prop.Id != group.Id);
                if (isExists)
                {
                    result.Success = false;
                    result.Message = "Group with same name already exists";
                    return result;
                }

                group.Name = request.Name;
                _dbContext.Group.Update(group);
            }

            await _dbContext.SaveChangesAsync();
            result.Success = true;

            return result;
        }
    }
}
