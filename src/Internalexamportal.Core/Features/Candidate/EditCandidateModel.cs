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
    public class EditCandidateModel : IRequest<EditCandidateResult>
    {
        public string Id { get; set; }
    }

    public class EditCandidateResult
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
        public int[] Group { get; set; }

    }

    public class EditCandidateResultHandler : IRequestHandler<EditCandidateModel, EditCandidateResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public EditCandidateResultHandler(
            IMapper mapper,
            UserManager<User> userData,
            InternalExamportalContext dbContext)
        {
            _mapper = mapper;
            _userManager = userData;
            _dbContext = dbContext;
        }

        public async Task<EditCandidateResult> Handle(EditCandidateModel candidate, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(candidate.Id);

            var result = _mapper.Map<EditCandidateResult>(user);

            result.Group = await _dbContext.CandidateGroup
                                    .Where(prop => prop.UserId == user.Id)
                                    .Select(prop => prop.GroupId)
                                    .ToArrayAsync();

            return result;
        }
    }
}
