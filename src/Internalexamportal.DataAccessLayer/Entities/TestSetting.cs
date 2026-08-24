using InternalExamportal.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
  public  class TestSetting : IAudited, ISoftDeleted
    {
        [Key]
        public int Id { get; set; }
        public int SettingId { get; set; }
        public Setting Setting { get; set; }
       
    }
}
