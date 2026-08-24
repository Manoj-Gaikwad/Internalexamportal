using AutoMapper;
using Internalexamportal.DataAccessLayer.Contracts;
using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Users
{
    public class GetUserModel : IRequest<GetUserResult>
    {
        public string Id { get; set; }
    }

    public class GetUserResult
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int ClientId { get; set; }
        public string Role { get; set; }

    }

    public class GetUserHandler : IRequestHandler<GetUserModel, GetUserResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public GetUserHandler(
            IMapper mapper,
            UserManager<User> userData)
        {
            _mapper = mapper;
            _userManager = userData;
        }

        public async Task<GetUserResult> Handle(GetUserModel candidate, CancellationToken cancellationToken)
        {
            var user = await  _userManager.FindByIdAsync(candidate.Id);

            var result = _mapper.Map<GetUserResult>(user);

            var roles = await _userManager.GetRolesAsync(user);

            result.Role = (roles.Count > 0) ? roles[0] : string.Empty;

            return  result;
        }
    }
}
