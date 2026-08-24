using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
  public class Subject:IAudited,ISoftDeleted
    {
        [Key]
        public int Id { get; set; }
        public string SubjectName { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public ICollection<SubjectTopic> SubjectTopic { get; set; }
    }
}
