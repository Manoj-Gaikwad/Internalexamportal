using AutoMapper;
using Internalexamportal.DataAccessLayer;
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
    public class GetTestResultsRequestModel : IRequest<GetTestResultsResult>
    {
        public int TestId { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string SearchTerm { get; set; }
        public string SortingColumn { get; set; }
        public string SortingDirection { get; set; }
    }
    public class GetTestResultsResult
    {
        public string TestName { get; set; }
        public List<GetTestResultsResponseModel> TestResult { get; set; }
        public int TotalCount { get; set; }
    }
    public class GetTestResultsResponseModel
    {
        public int Id { get; set; }
        public DateTime SubmitDate { get; set; }
        public int MaximumMark { get; set; }
        public int RightMark { get; set; }
        public int NegativeMark { get; set; }
        public string TotalMark { get; set; }
        public string UserName { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public bool AllowTestResume { get; set; }

    }

    public class GetTestResultsHandler : IRequestHandler<GetTestResultsRequestModel, GetTestResultsResult>
    {
        private readonly InternalExamportalContext _dbContext;
        private readonly IMapper _mapper;
        public GetTestResultsHandler(InternalExamportalContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetTestResultsResult> Handle(GetTestResultsRequestModel request, CancellationToken cancellationToken)
        {
            var result = new GetTestResultsResult();

            var results = _dbContext.SubmittedTest
                                    .Where(prop => prop.TestId == request.TestId)
                                    .Include(prop => prop.User)
                                    .Include(prop => prop.Status)
                                    .Include(prop => prop.Test)
                                    .OrderByDescending(prop => prop.SubmitDate);

            result.TestResult = _mapper.Map<List<GetTestResultsResponseModel>>(await Paginate(results,request));

            result.TestName = _dbContext.Test.Find(request.TestId).TestName;

            result.TotalCount = await results.CountAsync();

            return result;
        }


        private async Task<List<SubmittedTest>> Paginate(
          IQueryable<SubmittedTest> submittedTests,
          GetTestResultsRequestModel model)
        {
            //search
            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
                submittedTests = SearchSubmittedTests(submittedTests, model.SearchTerm);

            //sort
            if (!string.IsNullOrWhiteSpace(model.SortingColumn) &&
               !string.IsNullOrWhiteSpace(model.SortingDirection))
            {
                submittedTests = SortSubmittedTests(submittedTests, model.SortingColumn, model.SortingDirection);
            }

            if (model.PageNumber == 0) model.PageNumber = 1;

            //paginate
            return await submittedTests
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize)
                .ToListAsync();
        }

        private IQueryable<SubmittedTest> SearchSubmittedTests(
            IQueryable<SubmittedTest> submittedTests,
            string searchTerm)
        {
            searchTerm = searchTerm.ToLower();

            return submittedTests
                .Where(c => c.User.FirstName.StartsWith(searchTerm)
                    || c.User.LastName.StartsWith(searchTerm)
                    || c.User.Email.StartsWith(searchTerm)
                    || c.User.PhoneNumber.StartsWith(searchTerm)
                    );
        }

        private IQueryable<SubmittedTest> SortSubmittedTests(
           IQueryable<SubmittedTest> submittedTests,
           string sortColumn,
           string sortDirection)
        {
            switch (sortColumn)
            {
                case "name":
                    submittedTests = (sortDirection == "desc") ?
                    submittedTests.OrderByDescending(s => s.User.FullName) :
                    submittedTests.OrderBy(s => s.User.FullName);
                    break;
                case "submitDate":
                    submittedTests = (sortDirection == "desc") ?
                    submittedTests.OrderByDescending(s => s.SubmitDate) :
                    submittedTests.OrderBy(s => s.SubmitDate);
                    break;
                case "obtainedMarks":
                    submittedTests = (sortDirection == "desc") ?
                    submittedTests.OrderByDescending(s => s.ObtainedMarks) :
                    submittedTests.OrderBy(s => s.ObtainedMarks);
                    break;
                case "status":
                    submittedTests = (sortDirection == "desc") ?
                    submittedTests.OrderByDescending(s => s.StatusId) :
                    submittedTests.OrderBy(s => s.StatusId);
                    break;
            }

            return submittedTests;
        }
    }
}
