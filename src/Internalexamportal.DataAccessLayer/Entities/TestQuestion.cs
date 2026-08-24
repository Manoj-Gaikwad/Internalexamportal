using InternalExamportal.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
  public class TestQuestion : IAudited, ISoftDeleted
    {
        [Key]
        public int Id { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
         public int QuestionId { get; set; }
        public Question Question { get; set; }
        public int PositiveMark { get; set; }
        public int NegativeMark { get; set; }
    }
}
