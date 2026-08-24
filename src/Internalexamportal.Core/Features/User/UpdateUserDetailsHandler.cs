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
    public class UpdateUserDetailsModel : IRequest<UpdateUserDetailseResult>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Photo { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    public class UpdateUserDetailseResult
    {
        public bool Success { get; set; }
    }

    public class UpdateUserDetailsHandler : IRequestHandler<UpdateUserDetailsModel, UpdateUserDetailseResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<User> _userRepo;
        private readonly IMapper _mapper;

        public UpdateUserDetailsHandler(
            IMapper mapper,
            UserManager<User> userManager,
            IRepository<User> userRepo)
        {
            _mapper = mapper;
            _userManager = userManager;
            _userRepo = userRepo;
        }

        public async Task<UpdateUserDetailseResult> Handle(UpdateUserDetailsModel updateCandidate, CancellationToken cancellationToken)
        {
            var userData = await _userManager.FindByIdAsync(updateCandidate.Id);

            userData.FirstName = updateCandidate.FirstName;
            userData.LastName = updateCandidate.LastName;
            userData.PhoneNumber = updateCandidate.PhoneNumber;
            userData.DateOfBirth = updateCandidate.DateOfBirth;
            userData.Gender = updateCandidate.Gender;
            userData.Photo = updateCandidate.Photo;

            var result = await _userManager.UpdateAsync(userData);

            return new UpdateUserDetailseResult { Success = result.Succeeded};
        }
    }
}
