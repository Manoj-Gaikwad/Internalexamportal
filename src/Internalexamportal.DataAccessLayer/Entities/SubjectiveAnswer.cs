using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Contracts;
using System.ComponentModel.DataAnnotations;

namespace InternalExamportal.DataAccessLayer.Entities
{
    public class SubjectiveAnswer : IAudited, ISoftDeleted
    {
        [Key]
        public int Id { get; set; }
        public int SubmittedTestId { get; set; }
        public SubmittedTest Test { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public string Answer { get; set; }
        public int? ObtainedMarks { get; set; }
        public string Comment { get; set; }
        public double TimeTaken { get; set; }
    }
}
