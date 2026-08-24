using Aspose.Words;
using AutoMapper;
using Internalexamportal.Core.Extentions;
using Internalexamportal.Core.FileSystem;
using Internalexamportal.Core.Models;
using Internalexamportal.Core.Services;
using Internalexamportal.DataAccessLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.PrintFeatures
{
    public class PrintCandidateResultModel : IRequest<PrintCandidateResultResult>
    {
        public int Id { get; set; }
    }

    //Result
    public class PrintCandidateResultResult
    {
        public byte[] DocumentBlob { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
    }

    public class PrintCandidateResultHandler
        : IRequestHandler<PrintCandidateResultModel, PrintCandidateResultResult>
    {
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;
        private readonly InternalExamportalContext _dbContext;
        private readonly IDocumentStorage _documentStorage;

        public PrintCandidateResultHandler(
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

        public async Task<PrintCandidateResultResult> Handle(
            PrintCandidateResultModel request,
            CancellationToken cancellationToken)
        {
            var candidateResult = await _dbContext.SubmittedTest
                                                .Where(prop => prop.Id == request.Id)
                                                .Include(prop => prop.User)
                                                .Include(prop => prop.Test)
                                                    .ThenInclude(prop => prop.Client)
                                                .FirstOrDefaultAsync();

            var result = _mapper.Map<ResultPrintModel>(candidateResult);

            result.Percentage = candidateResult.Percentage.ToString("0.00");

            candidateResult.Test.Client.Users = null;

            var resultModel = new CandidateResultPrintModel
            {
                Result = result,
                PrintOutDate = DateTime.Now.ToString("dd/MM/yyyy"),
                Client = candidateResult.Test.Client
            };

            var templateModel = new TemplateModel
            {
                JsonFormData = JsonConvert.SerializeObject(resultModel)
            };

            string filePath = _documentStorage.GetDocumentPath(
                FileExtension.Docx,
                Templates.ExamResult);

            var fileResult = _documentService.GetTemplateStream(
                templateModel,
                SaveFormat.Pdf,
                filePath);

            return await Task.FromResult(new PrintCandidateResultResult
            {
                DocumentBlob = fileResult.ReadAllBytes(),
                MimeType = "application/pdf",
                FileName = result.UserName + "_" + candidateResult.Id
            });
        }
    }
}
