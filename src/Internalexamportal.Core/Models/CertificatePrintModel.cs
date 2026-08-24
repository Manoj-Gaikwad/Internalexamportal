using Internalexamportal.Core.Features.Admin.testManagement;
using InternalExamportal.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internalexamportal.Core.Models
{
    public class ClientDto
    {
        public string Name { get; set; }
    }
    public class CertificatePrintModelModel
    {
        public string Name { get; set; }
        public string ExamName { get; set; }
        public string SubmitDate { get; set; }
        public ClientDto Client  { get; set; }
    }

}
