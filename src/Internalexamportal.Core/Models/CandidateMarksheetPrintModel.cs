using Internalexamportal.Core.Features.Admin.testManagement;
using InternalExamportal.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Internalexamportal.Core.Models
{
    public class CandidateResultPrintModel
    {
        public ResultPrintModel Result { get; set; }
        public string PrintOutDate { get; set; }
        public Client Client { get; set; }
    }

    public class ResultPrintModel
    {
        public int Id { get; set; }
        public string CandidateId { get; set; }
        public string SubmitDate { get; set; }
        public string UserName { get; set; }
        public string TestName { get; set; }
        public string Email { get; set; }
        public int TotalMark { get; set; }
        public int ObtainedMark { get; set; }
        public string Percentage { get; set; }
        public int ObjectiveScore { get; set; }
        public int ObjectiveTotal { get; set; }
        public int SubjectiveScore { get; set; }
        public int SubjectiveTotal { get; set; }
    }
}
