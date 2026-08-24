using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
   public class SubjectTopic : IAudited, ISoftDeleted
    {
        public int Id { get; set; }
        public String Topic { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public ICollection<Question> Questions { get; set; } = new List<Question>();

    }
}
