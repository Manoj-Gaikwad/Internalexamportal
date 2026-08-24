using AutoMapper;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer;
using Internalexamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.Admin.testManagement
{
    public class GetTestModel : IRequest<GetTestModelResult>
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SearchTerm { get; set; }
        public string SortingColumn { get; set; }
        public string SortingDirection { get; set; }
    }

    public class GetTestModelResult
    {
        public List<TestModel> Tests { get; set; }
        public int TotalCount { get; set; }
      
    }

    public class TestModel
    {
        public int Id { get; set; }
        public string TestName { get; set; }
        public double Duration { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalMark { get; set; }
        public int TestUserCount { get; set; }
        public bool IsResultPublished { get; set; }
    }

    public class GetTestsHandler : IRequestHandler<GetTestModel, GetTestModelResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IClientManagerService _clientManager;
        public GetTestsHandler(InternalExamportalContext dbContext,
            IMapper mapper,
            IClientManagerService clientManager)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _clientManager = clientManager;
        }

        public async Task<GetTestModelResult> Handle(GetTestModel model, CancellationToken cancellationToken)
        {
            var tuple = await GetTests(model);

            var Tests = _mapper.Map<List<TestModel>>(await Paginate(tuple.Item1, model));

            return new GetTestModelResult
            {
                Tests = Tests,
                TotalCount = tuple.Item2
            };
        }


        private async Task<Tuple<IQueryable<Test>, int>> GetTests(
            GetTestModel model)
        {
            var client = await _clientManager.GetClientId();

            IQueryable<Test> test = null;
            int count = 0;

            test = _dbContext.Test
                            .Where(prop => client.Item2 || client.Item1 == prop.ClientId)
                            .Include(prop => prop.Setting)
                            .Include(pro => pro.SubmittedTest);

            count = await test.CountAsync();

            return new Tuple<IQueryable<Test>, int>(test, count);
        }


        private async Task<List<Test>> Paginate(
          IQueryable<Test> tests,
          GetTestModel model)
        {
            //search
            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
                tests = SearchTests(tests, model.SearchTerm);

            //sort
            if (!string.IsNullOrWhiteSpace(model.SortingColumn) &&
               !string.IsNullOrWhiteSpace(model.SortingDirection))
            {
                tests = SortTests(tests, model.SortingColumn, model.SortingDirection);
            }

            if (model.PageNumber == 0) model.PageNumber = 1;

            //paginate
            return await tests
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToListAsync();
        }

        private IQueryable<Test> SearchTests(
            IQueryable<Test> tests,
            string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return tests.Where(c => c.TestName.Contains(searchTerm));
        }

        private IQueryable<Test> SortTests(
           IQueryable<Test> tests,
           string sortColumn,
           string sortDirection)
        {
            switch (sortColumn)
            {

                case "testName":
                    tests = (sortDirection == "desc") ?
                    tests.OrderByDescending(s => s.TestName) :
                    tests.OrderBy(s => s.TestName);
                    break;
                case "duration":
                    tests = (sortDirection == "desc") ?
                    tests.OrderByDescending(s => s.Duration) :
                    tests.OrderBy(s => s.Duration);
                    break;
                case "totalQuestion":
                    tests = (sortDirection == "desc") ?
                    tests.OrderByDescending(s => s.TotalQuestion) :
                    tests.OrderBy(s => s.TotalQuestion);
                    break;
                case "totalMark":
                    tests = (sortDirection == "desc") ?
                    tests.OrderByDescending(s => s.TotalMark) :
                    tests.OrderBy(s => s.TotalMark);
                    break;
                case "testUserCount":
                    tests = (sortDirection == "desc") ?
                    tests.OrderByDescending(s => s.SubmittedTest.Count()) :
                    tests.OrderBy(s => s.SubmittedTest.Count());
                    break;

            }

            return tests;
        }

    }
}
