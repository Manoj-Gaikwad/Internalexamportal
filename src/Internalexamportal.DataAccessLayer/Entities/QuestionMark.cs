using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
   public class QuestionMark : IAudited, ISoftDeleted
    {
        [Key]
        public int Id { get; set; }
        public int RightMark { get; set; }
        public int NegativeMark { get; set; }
        public int DifficultLevel { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
