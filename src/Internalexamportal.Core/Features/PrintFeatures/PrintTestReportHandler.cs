using Aspose.Words;
using AutoMapper;
using Internalexamportal.Core.Commons.Utils;
using Internalexamportal.Core.Enums;
using Internalexamportal.Core.Extentions;
using Internalexamportal.Core.FileSystem;
using Internalexamportal.Core.Models;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.PrintFeatures
{
    public class PrintTestReportModel : IRequest<PrintTestReportResult>
    {
        public int Id { get; set; }
    }

    //Result
    public class PrintTestReportResult
    {
        public byte[] DocumentBlob { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
    }

    public class PrintTestReportHandler
        : IRequestHandler<PrintTestReportModel, PrintTestReportResult>
    {
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;
        private readonly InternalExamportalContext _dbContext;
        private readonly IDocumentStorage _documentStorage;

        public PrintTestReportHandler(
            IDocumentService documentService,
            InternalExamportalContext dbContext,
            IDocumentStorage documentStorage,
            IMapper mapper)
        {
            _documentService = documentService;
            _mapper = mapper;
            _dbContext = dbContext;
            _documentStorage = documentStorage;
        }

        public async Task<PrintTestReportResult> Handle(
            PrintTestReportModel request,
            CancellationToken cancellationToken)
        {
            var test = await _dbContext.Test.Where(prop => prop.Id == request.Id)
                                            .Include(prop => prop.Client)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync();

           

            var candidateResults = await _dbContext.SubmittedTest
                .Where(prop => prop.TestId == request.Id)
                .Where(prop => prop.StatusId == (int)TestStatusEnum.Evaluated)
                .Include(prop => prop.User)
                .Include(prop => prop.Test)
                .ToListAsync(cancellationToken);

             candidateResults = candidateResults
                            .OrderBy(prop => prop.User.FullName) // C# in-memory sort
                            .ToList();


            test.Client.Users = null;
            var results = _mapper.Map<List<ReportModel>>(candidateResults);
            int i = 1;
            foreach (var result in results)
            {
                result.Id = i;
                i++;
            }

            var resultModel = new ReportPrintModel
            {
                Client = test.Client,
                Test = _mapper.Map<TestDetailsModel>(test),
                Results = results,
                PrintDate = Utilities.GetISTDateTime().ToString("DD/MM/YYYY")
            };

            var templateModel = new TemplateModel
            {
                JsonFormData = JsonConvert.SerializeObject(resultModel)
            };

            string filePath = _documentStorage.GetDocumentPath(
                FileExtension.Docx,
                Templates.ExamReport);

            var fileResult = _documentService.GetTemplateStream(
                templateModel,
                SaveFormat.Pdf,
                filePath);

            return await Task.FromResult(new PrintTestReportResult
            {
                DocumentBlob = fileResult.ReadAllBytes(),
                MimeType = "application/pdf",
                FileName = test.TestName + "_" + resultModel.PrintDate
            });
        }
    }
}
