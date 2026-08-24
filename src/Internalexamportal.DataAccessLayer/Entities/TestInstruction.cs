using InternalExamportal.DataAccessLayer.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
   public class TestInstruction : IAudited, ISoftDeleted
    {
        [Key]
        public int Id { get; set; }
        public string Instruction { get; set; }
        public string InstructionDescripiton { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
    }

}
