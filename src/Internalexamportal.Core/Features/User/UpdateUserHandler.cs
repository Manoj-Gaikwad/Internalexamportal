using AutoMapper;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using Internalexamportal.DataAccessLayer.Entities;
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
    public class UpdateUserModel : IRequest<UpdateUserResult>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public string Role { get; set; }
        public int ClientId { get; set; }
    }

    public class UpdateUserResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class UpdateUserHandler : IRequestHandler<UpdateUserModel, UpdateUserResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateUserHandler(
            IMapper mapper,
            UserManager<User> userManager,
            InternalExamportalContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<UpdateUserResult> Handle(UpdateUserModel request, CancellationToken cancellationToken)
        {
            var result = new UpdateUserResult();

            var isExists = await _dbContext.Users.AnyAsync(prop => prop.Email == request.Email && prop.Id != request.Id);

            if (isExists)
            {
                result.Success = false;
                result.Message = "User with same email already exists";
                return result;
            }
            
            var user = await _userManager.FindByIdAsync(request.Id);

            user.UserName = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.PhoneNumber = request.PhoneNumber;
            user.DateOfBirth = request.DOB;
            user.Gender = request.Gender;
            user.ClientId = request.ClientId;

            var identityResult = await _userManager.UpdateAsync(user);

            //update role for user
            if (request.Role != null)
            {
                // check if user has role
                var roles = await _userManager.GetRolesAsync(user);
                if (!request.Role.Contains(roles[0].ToString()))
                {
                    //remove old role
                    var userRole = _dbContext.UserRoles.Where(u => u.UserId == user.Id).FirstOrDefault();
                    _dbContext.UserRoles.Remove(userRole);

                    //if not assign role
                    await _userManager
                        .AddToRoleAsync(user, request.Role);
                }
            }

            return new UpdateUserResult { Success = identityResult.Succeeded};
        }
    }
}
