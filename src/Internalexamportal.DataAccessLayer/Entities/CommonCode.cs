using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
  public  class CommonCode
    {
        [Key]
        public int Id { get; set; }
        public string CommonTestCode { get; set; }
        public int ActivationCodeId { get; set; }
        public ActivationCode ActivationCode { get; set; }
    }
}
