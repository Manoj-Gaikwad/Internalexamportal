using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Contracts;
using System.ComponentModel.DataAnnotations;

namespace InternalExamportal.DataAccessLayer.Entities
{
    public class SubmittedOption
    {
        [Key]
        public int Id { get; set; }
        public int SubmittedTestId { get; set; }
        public SubmittedTest Test { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int QuestionOptionId { get; set; }
        public QuestionOption QuestionOption { get; set; }
        public double TimeTaken { get; set; }

    }
}
