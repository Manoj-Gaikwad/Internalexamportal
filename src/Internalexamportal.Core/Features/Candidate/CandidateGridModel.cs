using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Services;
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

namespace Internalexamportal.Core.Features.Candidate
{
    public class CandidateGridModel : IRequest<GetCandidateResult>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SearchTerm { get; set; }
        public int[] Groups { get; set; }
        public string SortingColumn { get; set; }
        public string SortingDirection { get; set; }
    }
    public class GetCandidateResult
    {
        public List<CandidateModel> Candidates { get; set; }
        public int TotalCount { get; set; }
    }

    public class CandidateModel
    {
        public Guid Id { get; set; }
        public string RollNumber { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Group { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
    }
    public class GetCandidateHandler : IRequestHandler<CandidateGridModel, GetCandidateResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IClientManagerService _clientManager;

        public GetCandidateHandler(UserManager<User> userManager,
            IMapper mapper,
            InternalExamportalContext dbContext,
            IClientManagerService clientManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dbContext = dbContext;
            _clientManager = clientManager;
        }

        public async Task<GetCandidateResult> Handle(CandidateGridModel request, CancellationToken cancellationToken)
        {
            var result = new GetCandidateResult();

            var candidateIds = _userManager.GetUsersInRoleAsync(GlobalConstants.CandidateRoleName)
                                        .Result.Select(prop => prop.Id).ToArray();

            var client = await _clientManager.GetClientId();

            var candidates = _dbContext.Users
                                 .Include(prop => prop.CandidateGroup)
                                     .ThenInclude(prop => prop.Group)
                                 .Where(prop => candidateIds.Contains(prop.Id))
                                 .Where(prop => !prop.IsDelete)
                                 .Where(prop => client.Item2 || client.Item1 == prop.ClientId)
                                 .Where(prop => request.Groups == null
                                             || !request.Groups.Any()
                                             || prop.CandidateGroup.Any(cg => request.Groups.Contains(cg.GroupId)));


            result.Candidates = _mapper.Map<List<CandidateModel>>(await Paginate(candidates, request));

            result.TotalCount = candidates.Count();

            return result;
        }


        private async Task<List<User>> Paginate(
          IQueryable<User> candidates,
          CandidateGridModel model)
        {
            //search
            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
                candidates = SearchCandidates(candidates, model.SearchTerm);

            //sort
            if (!string.IsNullOrWhiteSpace(model.SortingColumn) &&
               !string.IsNullOrWhiteSpace(model.SortingDirection))
            {
                candidates = SortCandidates(candidates, model.SortingColumn, model.SortingDirection);
            }

            if (model.PageNumber == 0) model.PageNumber = 1;

            //paginate
            return await candidates
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToListAsync();
        }

        private IQueryable<User> SearchCandidates(
            IQueryable<User> candidates,
            string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return candidates
                .Where(c => c.Email.Contains(searchTerm)
                    || c.FirstName.Contains(searchTerm)
                    || c.LastName.Contains(searchTerm)
                    || c.PhoneNumber.Contains(searchTerm)
                    || c.RollNumber.StartsWith(searchTerm)
                    );
        }

        private IQueryable<User> SortCandidates(
           IQueryable<User> candidates,
           string sortColumn,
           string sortDirection)
        {
            switch (sortColumn)
            {

                case "name":
                    candidates = (sortDirection == "desc") ?
                    candidates.OrderByDescending(s => s.FirstName) :
                    candidates.OrderBy(s => s.FirstName);
                    break;
                case "email":
                    candidates = (sortDirection == "desc") ?
                    candidates.OrderByDescending(s => s.Email) :
                    candidates.OrderBy(s => s.Email);
                    break;
                case "phoneNumber":
                    candidates = (sortDirection == "desc") ?
                    candidates.OrderByDescending(s => s.PhoneNumber) :
                    candidates.OrderBy(s => s.PhoneNumber);
                    break;
                case "date":
                    candidates = (sortDirection == "desc") ?
                    candidates.OrderByDescending(s => s.RegistrationDate) :
                    candidates.OrderBy(s => s.RegistrationDate);
                    break;
                case "status":
                    candidates = (sortDirection == "desc") ?
                    candidates.OrderByDescending(s => s.IsActive) :
                    candidates.OrderBy(s => s.IsActive);
                    break;
                case "rollNumber":
                    candidates = (sortDirection == "desc") ?
                    candidates.OrderByDescending(s => s.RollNumber) :
                    candidates.OrderBy(s => s.RollNumber);
                    break;

            }

            return candidates;
        }
    }

}
