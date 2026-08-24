using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
    public class QuestionOption : IAudited, ISoftDeleted
    {
        [Key]
        public int Id { get; set; }
        public String OptionText { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public bool IsAnswer { get; set; }
    }
}