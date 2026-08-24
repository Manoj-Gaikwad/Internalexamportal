using InternalExamportal.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
    public class Test : IAudited, ISoftDeleted
    {

        [Key]
        public int Id { get; set; }
        public string TestName { get; set; }
        public double Duration { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalMark { get; set; }
        public float? Percentage { get; set; }
        public int TestTypeId { get; set; }
        public TestType TestType { get; set; }
        public int TestInstructionId { get; set; }
        public TestInstruction TestInstruction { get; set; }
        public int DifficultLevelId { get; set; }
        public DifficultLevel DifficultLevel { get; set; }
        public Guid LinkId{get;set;}
        public ICollection<ActivationCode> ActivationCode { get; set; }
        public TestPublish TestPublish { get; set; }
        public ICollection<TestQuestion> TestQuestion { get; set; }
        public ICollection<Setting> Setting { get; set; }
        public ICollection<SubmittedTest> SubmittedTest { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }
}
