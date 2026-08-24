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

namespace Internalexamportal.Core.Features.Candidate
{
    public class CandidateDetailModel : IRequest<CandidateDetaileResult>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? DOB { get; set; }
        public int ClientId { get; set; }
        public int[] Group { get; set; }
    }

    public class CandidateDetaileResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class UpdateCandidateResultHandler : IRequestHandler<CandidateDetailModel, CandidateDetaileResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateCandidateResultHandler(
            IMapper mapper,
            UserManager<User> userManager,
            InternalExamportalContext dbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<CandidateDetaileResult> Handle(CandidateDetailModel request, CancellationToken cancellationToken)
        {
            var result = new CandidateDetaileResult();

            var isExists = await _dbContext.Users.AnyAsync(prop => prop.Email == request.Email && prop.Id != request.Id);

            if (isExists)
            {
                result.Success = false;
                result.Message = "User with same email already exists";
                return result;
            }

            var userData = await _userManager.FindByIdAsync(request.Id);

            userData.UserName = request.Email;
            userData.FirstName = request.FirstName;
            userData.LastName = request.LastName;
            userData.Email = request.Email;
            userData.PhoneNumber = request.PhoneNumber;
            userData.DateOfBirth = request.DOB;
            userData.Gender = request.Gender;
            userData.ClientId = request.ClientId;

            var identityResult = await _userManager.UpdateAsync(userData);

            //Remove previous groups
            var oldGroups = _dbContext.CandidateGroup.Where(prop => prop.UserId == userData.Id);
            _dbContext.CandidateGroup.RemoveRange(oldGroups);
            await _dbContext.SaveChangesAsync();

            //Add new groups
            foreach (int group in request.Group)
            {
                _dbContext.CandidateGroup.Add(new CandidateGroup { GroupId = group, UserId = userData.Id });
            }
            await _dbContext.SaveChangesAsync();

            return new CandidateDetaileResult { Success = identityResult.Succeeded };
        }
    }
}
