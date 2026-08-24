using Internalexamportal.Core.Features.Admin.testManagement;
using InternalExamportal.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internalexamportal.Core.Models
{
    public class ReportPrintModel
    {
        public Client Client { get; set; }
        public TestDetailsModel Test { get; set; }
        public List<ReportModel> Results { get; set; }
        public string PrintDate { get; set; }
    }

    public class TestDetailsModel
    {
        public string Name { get; set; }
        public int TotalMarks { get; set; }
    }

    public class ReportModel
    {
        public int Id { get; set; }
        public string RollNumber { get; set; }
        public string Name { get; set; }
        public int Marks { get; set; }
        public string Percentage { get; set; }
        public string Result { get; set; }
    }

}
