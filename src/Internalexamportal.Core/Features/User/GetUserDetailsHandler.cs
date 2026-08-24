using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
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

namespace Internalexamportal.Core.Features.Users
{
    public class GetUserDetailsModel : IRequest<GetUserDetailsResult>
    {
        public string Id { get; set; }
    }

    public class GetUserDetailsResult
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Photo { get; set; }
        public string ClientName { get; set; }

    }

    public class GetUserDetailsResultHandler : IRequestHandler<GetUserDetailsModel, GetUserDetailsResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly InternalExamportalContext _dbContext;

        public GetUserDetailsResultHandler(
            IMapper mapper,
            UserManager<User> userData,
            InternalExamportalContext dbContext)
        {
            _mapper = mapper;
            _userManager = userData;
            _dbContext = dbContext;
        }

        public async Task<GetUserDetailsResult> Handle(GetUserDetailsModel model, CancellationToken cancellationToken)
        {
            var user = await  _dbContext.Users
                                        .Where(prop => prop.Id == model.Id)
                                        .Include(prop => prop.Client)
                                        .FirstOrDefaultAsync();

            var result = _mapper.Map<GetUserDetailsResult>(user);

            return  result;
        }
    }
}
