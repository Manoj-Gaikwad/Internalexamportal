using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
  public  class Status
    {
        [Key]
        public int StatusId { get; set; }
        public string Inprogress { get; set;}
        public string Onhold { get; set; }
        public string Suspended { get; set; }
        public string Finish { get; set; }
        public string Pass { get; set; }
        public string Fail { get; set; }
    }
}
