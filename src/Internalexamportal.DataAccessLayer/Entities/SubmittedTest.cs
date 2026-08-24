using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
    public class SubmittedTest : IAudited, ISoftDeleted
    {
        [Key]
        public int Id { get; set; }
        public DateTime SubmitDate { get; set; }
        public int AttemptedQuestion { get; set; }
        public int SkippedQuestion { get; set; }
        public int ReviewedQuestion { get; set; }
        public int CorrectQuestion { get; set; }
        public int InCorrectQuestion { get; set; }
        public int MaximumMark { get; set; }
        public int RightMark { get; set; }
        public int NegativeMark { get; set; }
        public int TotalMark { get; set; }
        public double TimeTaken { get; set; }
        public bool AllowTestResume { get; set; }
        public int StatusId { get; set; }
        public TestStatus Status { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public ICollection<SubjectiveAnswer> SubjectiveAnswers { get; set; }
        public ICollection<SubmittedOption> SubmittedOptions { get; set; }

        [NotMapped]
        public int ObtainedMarks { get => RightMark - NegativeMark; }
        [NotMapped]
        public float Percentage { get => (float)decimal.Divide(ObtainedMarks,MaximumMark)*100; }

    }
}
