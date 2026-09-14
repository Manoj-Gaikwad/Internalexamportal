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

        public GetAdminGridHandler(
            UserManager<User> userManager,
            IMapper mapper,
            InternalExamportalContext dbContext,
            IClientManagerService clientManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _dbContext = dbContext;
            _clientManager = clientManager;
        }

        public async Task<GetAdminGridResult> Handle(
            AdminGridModel request,
            CancellationToken cancellationToken)
        {
            var client = await _clientManager.GetClientId();

            // Materialize roles before using GetUsersInRoleAsync().
            // This prevents NpgsqlOperationInProgressException because
            // the Roles query is completed before another query starts.
            var roles = await _dbContext.Roles
                .Where(prop => prop.Name != GlobalConstants.CandidateRoleName)
                .Where(prop => prop.Name != GlobalConstants.SuperAdminRoleName)
                .ToListAsync(cancellationToken);

            var users = new List<AdminModel>();

            foreach (var role in roles)
            {
                var roleUsers = await _userManager.GetUsersInRoleAsync(role.Name);

                roleUsers = roleUsers
                    .Where(prop => !prop.IsDelete)
                    .Where(prop => client.Item2 || client.Item1 == prop.ClientId)
                    .ToList();

                var rUsers = _mapper.Map<IEnumerable<AdminModel>>(roleUsers);

                foreach (var user in rUsers)
                {
                    user.Role = role.Name;
                }

                users.AddRange(rUsers);
            }

            // Search
            IEnumerable<AdminModel> admins = users;

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                admins = SearchAdmins(admins, request.SearchTerm);
            }

            // Sort
            if (!string.IsNullOrWhiteSpace(request.SortingColumn) &&
                !string.IsNullOrWhiteSpace(request.SortingDirection))
            {
                admins = SortAdmins(
                    admins,
                    request.SortingColumn,
                    request.SortingDirection);
            }

            // Total count after search but before pagination
            var totalCount = admins.Count();

            // Page number
            var pageNumber = request.PageNumber <= 0
                ? 1
                : request.PageNumber;

            // Page size
            var pageSize = request.PageSize <= 0
                ? 10
                : request.PageSize;

            // Pagination
            var pagedAdmins = admins
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new GetAdminGridResult
            {
                Admins = pagedAdmins,
                TotalCount = totalCount
            };
        }

        private IEnumerable<AdminModel> SearchAdmins(
            IEnumerable<AdminModel> admins,
            string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return admins.Where(c =>
                (!string.IsNullOrEmpty(c.Email) &&
                 c.Email.ToLower().Contains(searchTerm))
                ||
                (!string.IsNullOrEmpty(c.FirstName) &&
                 c.FirstName.ToLower().Contains(searchTerm))
                ||
                (!string.IsNullOrEmpty(c.LastName) &&
                 c.LastName.ToLower().Contains(searchTerm))
                ||
                (!string.IsNullOrEmpty(c.PhoneNumber) &&
                 c.PhoneNumber.ToLower().Contains(searchTerm))
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
                    admins = sortDirection == "desc"
                        ? admins.OrderByDescending(s => s.FirstName)
                        : admins.OrderBy(s => s.FirstName);
                    break;

                case "email":
                    admins = sortDirection == "desc"
                        ? admins.OrderByDescending(s => s.Email)
                        : admins.OrderBy(s => s.Email);
                    break;

                case "phoneNumber":
                    admins = sortDirection == "desc"
                        ? admins.OrderByDescending(s => s.PhoneNumber)
                        : admins.OrderBy(s => s.PhoneNumber);
                    break;

                case "role":
                    admins = sortDirection == "desc"
                        ? admins.OrderByDescending(s => s.Role)
                        : admins.OrderBy(s => s.Role);
                    break;

                case "status":
                    admins = sortDirection == "desc"
                        ? admins.OrderByDescending(s => s.IsActive)
                        : admins.OrderBy(s => s.IsActive);
                    break;
            }

            return admins;
        }
    }
}
