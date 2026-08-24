using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Internalexamportal.DataAccessLayer.Contracts;
using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Internalexamportal.DataAccessLayer;

namespace Internalexamportal.Core.Features.ClientFeatures
{
    public class DeleteClient : IRequest<ClientDeleteResult>
    {
              public string Id { get; set; }
    }
           
        public class ClientDeleteResult
    {

        }
        public class DeleteClientHandler : IRequestHandler<DeleteClient, ClientDeleteResult>
        {

            private readonly IRepository<Client> _client;
            private readonly InternalExamportalContext _dbContext;
            private readonly IMapper _mapper;
            public DeleteClientHandler(IRepository<Client> client, UserManager<User> userManager, InternalExamportalContext dbContext,  IMapper mapper)
            {
                _client = client;
                _mapper = mapper;
                _dbContext = dbContext;
        
        }

        public async Task<ClientDeleteResult> Handle(DeleteClient request, CancellationToken cancellationToken)
        {
            ClientDeleteResult result = new ClientDeleteResult();
            var id = int.Parse(request.Id);
            var clientId = _client.Find(id);
          
            if (clientId != null)
            {
                _client.Remove(clientId);
                _client.SaveChanges();
                           

                var userList = _dbContext.Users.Where(prop => prop.ClientId.Equals(clientId.Id))
                                .ToList();
               
              // _dbContext.Users.RemoveRange(userList);
                            
                foreach(var res in userList)
                {
                    res.IsDelete=true;
                }

               // _dbContext.Users.Where(prop => prop.IsDelete==true);

                foreach (var i in userList)
                {
                   var candidateList = _dbContext.CandidateGroup.Where(prop => prop.UserId == i.Id).ToList();

                    _dbContext.CandidateGroup.RemoveRange(candidateList);
                }
             
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

           
            return result;
        }
    }
    }






