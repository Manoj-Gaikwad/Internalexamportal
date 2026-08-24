using InternalExamportal.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
  public class Setting:IAudited, ISoftDeleted
    {
        [Key]
        public int Id { get; set; }
        public int TestSettingTypeId { get; set; }
        public TestSettingType TestSettingType{ get; set; }
        public bool IsSettingApplied { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
    }
}
