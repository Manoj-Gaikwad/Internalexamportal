using InternalExamportal.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
    public class TestPublish : IAudited, ISoftDeleted
    {
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
    }
}
