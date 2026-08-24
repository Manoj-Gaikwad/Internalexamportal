using Aspose.Cells;
using Aspose.Pdf.Operators;
using AutoMapper;
using Internalexamportal.Core.Enums;
using Internalexamportal.Core.Models;
using Internalexamportal.DataAccessLayer;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Threading;
using System.Threading.Tasks;

namespace Internalexamportal.Core.Features.PrintFeatures
{
    // Request model
    public class PrintTestReportExcelModel : IRequest<PrintTestReportExcelResult>
    {
        public int Id { get; set; }
    }

    // Result model
    public class PrintTestReportExcelResult
    {
        public byte[] DocumentBlob { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
    }

    // Handler
    public class PrintTestReportExcelHandler : IRequestHandler<PrintTestReportExcelModel, PrintTestReportExcelResult>
    {
        private readonly IMapper _mapper;
        private readonly InternalExamportalContext _dbContext;

        public PrintTestReportExcelHandler(
            InternalExamportalContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PrintTestReportExcelResult> Handle(PrintTestReportExcelModel request, CancellationToken cancellationToken)
        {
            var test = await _dbContext.Test
                .Where(t => t.Id == request.Id)
                .Include(t => t.Client)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);

            if (test == null)
                throw new Exception("Test not found.");

            var candidateResults = await _dbContext.SubmittedTest
                .Where(st => st.TestId == request.Id && st.StatusId == (int)TestStatusEnum.Evaluated)
                .Include(st => st.User)
                .Include(st => st.Test)
                .ToListAsync(cancellationToken);

            candidateResults = candidateResults.OrderBy(st => st.User.FullName).ToList();

            var results = _mapper.Map<List<ReportModel>>(candidateResults);

            int counter = 1;
            foreach (var result in results)
                result.Id = counter++;

            // Create Excel workbook
            var workbook = new Workbook();
            var sheet = workbook.Worksheets[0];
            sheet.Name = "Test Report";

            // Add header
            sheet.Cells[0, 0].Value = "S.No";
            sheet.Cells[0, 1].Value = "Roll Number";
            sheet.Cells[0, 2].Value = "Name";
            sheet.Cells[0, 3].Value = "Marks";
            sheet.Cells[0, 4].Value = "Percentage";
            sheet.Cells[0, 5].Value = "Result";

            // Add data rows
            for (int i = 0; i < results.Count; i++)
            {
                var r = results[i];
                sheet.Cells[i + 1, 0].Value = r.Id;
                sheet.Cells[i + 1, 1].Value = r.RollNumber;
                sheet.Cells[i + 1, 2].Value = r.Name;
                sheet.Cells[i + 1, 3].Value = r.Marks;
                sheet.Cells[i + 1, 4].Value = r.Percentage;
                sheet.Cells[i + 1, 5].Value = r.Result;
            }

            sheet.AutoFitColumns();

            byte[] fileBytes;
            using (var stream = new System.IO.MemoryStream())
            {
                workbook.Save(stream, SaveFormat.Xlsx);
                fileBytes = stream.ToArray();
            }

            return new PrintTestReportExcelResult
            {
                DocumentBlob = fileBytes,
                MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileName = $"{test.TestName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx"
            };
        }
    }
}


  

