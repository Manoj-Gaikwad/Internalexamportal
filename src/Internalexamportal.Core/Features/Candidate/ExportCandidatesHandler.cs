using AutoMapper;
using Internalexamportal.Core.Commons;
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
    public class ExportCandidatesModel : IRequest<ExportCandidatesResult>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SearchTerm { get; set; }
        public int[] Groups { get; set; }
        public string SortingColumn { get; set; }
        public string SortingDirection { get; set; }
    }
    public class ExportCandidatesResult
    {
        public List<ExportCandidateModel> Candidates { get; set; }
        public int TotalCount { get; set; }
    }


    public class ExportCandidateModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Group { get; set; }
        public string RegistrationDate { get; set; }
    }

    public class ExportCandidatesHandler : IRequestHandler<ExportCandidatesModel, ExportCandidatesResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;

        public ExportCandidatesHandler(UserManager<User> userManager, IMapper mapper, InternalExamportalContext dbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<ExportCandidatesResult> Handle(ExportCandidatesModel request, CancellationToken cancellationToken)
        {
            var result = new ExportCandidatesResult();

            var candidateIds =  _userManager.GetUsersInRoleAsync(GlobalConstants.CandidateRoleName)
                                        .Result.Select(prop =>prop.Id).ToArray();

           var candidates = _dbContext.Users
                                .Include(prop => prop.CandidateGroup)
                                    .ThenInclude(prop => prop.Group)
                                .Where(prop => candidateIds.Contains(prop.Id))
                                .Where(prop => !prop.IsDelete)
                                .Where(prop => request.Groups == null 
                                            || !request.Groups.Any() 
                                            || prop.CandidateGroup.Any(cg => request.Groups.Contains(cg.GroupId)));


            result.Candidates = _mapper.Map<List<ExportCandidateModel>>(await Paginate(candidates, request));

            result.TotalCount = candidates.Count();

            return result;
        }


        private async Task<List<User>> Paginate(
          IQueryable<User> candidates,
          ExportCandidatesModel model)
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

            //paginate
            return await candidates.ToListAsync();
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

            }

            return candidates;
        }
    }

}
