using AutoMapper;
using Internalexamportal.Core.Commons;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Contracts;
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

namespace Internalexamportal.Core.Features.Admin.Dashboard
{
    public class GetDashboardDataModel : IRequest<GetDashboardDataModelResult>
    {
    }

    public class GetDashboardDataModelResult
    {
        public int TestCount { get; set; }
        public int CandidateCount { get; set; }
        public int QuestionsCount { get; set; }
        public int SubjectCount { get; set; }

    }

    public class GetDashboardDatasHandler : IRequestHandler<GetDashboardDataModel, GetDashboardDataModelResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IClientManagerService _clientManager;

        public GetDashboardDatasHandler(InternalExamportalContext dbContext,
            IMapper mapper,
            UserManager<User> userManager,
            IClientManagerService clientManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
            _clientManager = clientManager;
        }

        public async Task<GetDashboardDataModelResult> Handle(GetDashboardDataModel request, CancellationToken cancellationToken)
        {
            var result = new GetDashboardDataModelResult();

            var client = await _clientManager.GetClientId();

            result.TestCount = _dbContext.Test
                                        .Where(prop => client.Item2 || client.Item1 == prop.ClientId)
                                        .Count();

            var candidates = await _userManager
                                .GetUsersInRoleAsync(GlobalConstants.CandidateRoleName);

            result.CandidateCount = candidates.Where(prop => !prop.IsDelete)
                                 .Where(prop => client.Item2 || client.Item1 == prop.ClientId)
                                 .Count();

            result.SubjectCount = _dbContext.Subject
                                        .Where(prop => client.Item2 || client.Item1 == prop.ClientId)
                                        .Count();

            result.QuestionsCount = await _dbContext.Question
                                    .Where(q => !q.IsDeleted)
                                    .Where(q => client.Item2 || q.ClientId == client.Item1)
                                    .CountAsync(cancellationToken);


            return result;
        }


    }
}
