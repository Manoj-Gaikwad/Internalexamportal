using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
  public class AccessCode
    {
        [Key]
        public int Id { get; set; }
        public string AccessTestCode { get; set; }
        public string Email { get; set; }
        public int ActivationCodeId { get; set; }
        public ActivationCode ActivationCode { get; set; }
    }
}
