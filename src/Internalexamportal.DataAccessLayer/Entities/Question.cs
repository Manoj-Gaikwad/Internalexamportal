using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
   public class Question :IAudited,ISoftDeleted
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public int SubjectTopicId { get; set; }
        public bool IsDeleted { get; set; }
        public SubjectTopic SubjectTopic { get; set; }
        public int QuestionTypeId { get; set; }
        public QuestionType QuestionType { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public ICollection<QuestionOption> QuestionOption { get; set; }
        public QuestionMark QuestionMark { get; set; }
        public QuestionAnswer QuestionAnswer { get; set; }
        public ICollection<TestQuestion> TestQuestion { get; set; }

    }
}
