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
    public class AdminGridModel : IRequest<GetAdminGridResult>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SearchTerm { get; set; }
        public string SortingColumn { get; set; }
        public string SortingDirection { get; set; }
    }
    public class GetAdminGridResult
    {
        public List<AdminModel> Admins { get; set; }
        public int TotalCount { get; set; }
    }

    public class AdminModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
    public class GetAdminGridHandler : IRequestHandler<AdminGridModel, GetAdminGridResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IClientManagerService _clientManager;


        public GetAdminGridHandler(UserManager<User> userManager,
            IMapper mapper,
            InternalExamportalContext dbContext,
            IClientManagerService clientManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dbContext = dbContext;
            _clientManager = clientManager;
        }

        public async Task<GetAdminGridResult> Handle(AdminGridModel request, CancellationToken cancellationToken)
        {
            var result = new GetAdminGridResult();

            var client = await _clientManager.GetClientId();

            var roles = _dbContext.Roles
                .Where(prop => prop.Name != GlobalConstants.CandidateRoleName)
                .Where(prop => prop.Name != GlobalConstants.SuperAdminRoleName);

            List<AdminModel> users = new List<AdminModel>();

            foreach (var role in roles)
            {
                var roleUsers = await _userManager.GetUsersInRoleAsync(role.Name);

                roleUsers = roleUsers.Where(prop => !prop.IsDelete)
                                    .Where(prop => client.Item2 || client.Item1 == prop.ClientId)
                                    .ToList();

                var rUsers = _mapper.Map<IEnumerable<AdminModel>>(roleUsers);

                foreach (var user in rUsers)
                {
                    user.Role = role.Name;
                }

                users.AddRange(rUsers);
            }

            result.Admins = await Paginate(users, request);

            result.TotalCount = users.Count();

            return result;
        }


        private async Task<List<AdminModel>> Paginate(
          IEnumerable<AdminModel> admins,
          AdminGridModel model)
        {
            //search
            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
                admins = SearchAdmins(admins, model.SearchTerm);

            //sort
            if (!string.IsNullOrWhiteSpace(model.SortingColumn) &&
               !string.IsNullOrWhiteSpace(model.SortingDirection))
            {
                admins = SortAdmins(admins, model.SortingColumn, model.SortingDirection);
            }

            if (model.PageNumber == 0) model.PageNumber = 1;

            //paginate
            return admins
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToList();
        }

        private IEnumerable<AdminModel> SearchAdmins(
            IEnumerable<AdminModel> admins,
            string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return admins
                .Where(c => c.Email.Contains(searchTerm)
                    || c.FirstName.Contains(searchTerm)
                    || c.LastName.Contains(searchTerm)
                    || c.PhoneNumber.Contains(searchTerm)
                    );
        }

        private IEnumerable<AdminModel> SortAdmins(
           IEnumerable<AdminModel> admins,
           string sortColumn,
           string sortDirection)
        {
            switch (sortColumn)
            {

                case "name":
                    admins = (sortDirection == "desc") ?
                    admins.OrderByDescending(s => s.FirstName) :
                    admins.OrderBy(s => s.FirstName);
                    break;
                case "email":
                    admins = (sortDirection == "desc") ?
                    admins.OrderByDescending(s => s.Email) :
                    admins.OrderBy(s => s.Email);
                    break;
                case "phoneNumber":
                    admins = (sortDirection == "desc") ?
                    admins.OrderByDescending(s => s.PhoneNumber) :
                    admins.OrderBy(s => s.PhoneNumber);
                    break;
                case "role":
                    admins = (sortDirection == "desc") ?
                    admins.OrderByDescending(s => s.Role) :
                    admins.OrderBy(s => s.Role);
                    break;
                case "status":
                    admins = (sortDirection == "desc") ?
                    admins.OrderByDescending(s => s.IsActive) :
                    admins.OrderBy(s => s.IsActive);
                    break;

            }

            return admins;
        }
    }

}
