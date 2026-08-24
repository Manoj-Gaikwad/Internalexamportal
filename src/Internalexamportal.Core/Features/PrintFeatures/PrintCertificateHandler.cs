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
    public class PrintCertificateModel : IRequest<PrintCertificateResult>
    {
        public int Id { get; set; }
    }

    //Result
    public class PrintCertificateResult
    {
        public byte[] DocumentBlob { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
    }

    public class PrintCertificateHandler
        : IRequestHandler<PrintCertificateModel, PrintCertificateResult>
    {
        private readonly IDocumentService _documentService;
        private readonly IMapper _mapper;
        private readonly InternalExamportalContext _dbContext;
        private readonly IDocumentStorage _documentStorage;

        public PrintCertificateHandler(
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

        public async Task<PrintCertificateResult> Handle(
            PrintCertificateModel request,
            CancellationToken cancellationToken)
        {
            var candidateCertificate = await _dbContext.SubmittedTest
                                                .Where(prop => prop.Id == request.Id)
                                                .Include(prop => prop.User)
                                                .Include(prop => prop.Test)
                                                    .ThenInclude(prop => prop.Client)
                                                .Select(prop => new CertificatePrintModelModel
                                                {
                                                    SubmitDate = prop.SubmitDate.ToString("dd/MM/yyyy"),
                                                    Name = prop.User.FullName,
                                                    ExamName = prop.Test.TestName,
                                                    Client = new ClientDto
                                                    {
                                                        Name = prop.Test.Client.Name.ToUpper() // Only serialize the Name
                                                    }
                                                }).FirstOrDefaultAsync();

            candidateCertificate.Client.Name = candidateCertificate.Client.Name.ToUpper();

            var templateModel = new TemplateModel
            {
                JsonFormData = JsonConvert.SerializeObject(candidateCertificate)
            };

            string filePath = _documentStorage.GetDocumentPath(
                FileExtension.Docx,
                Templates.ExamCertificate);

            var fileResult = _documentService.GetTemplateStream(
                templateModel,
                SaveFormat.Pdf,
                filePath);

            return await Task.FromResult(new PrintCertificateResult
            {
                DocumentBlob = fileResult.ReadAllBytes(),
                MimeType = "application/pdf",
                FileName = candidateCertificate.Name + "_" + candidateCertificate.ExamName
            });
        }
    }
}
