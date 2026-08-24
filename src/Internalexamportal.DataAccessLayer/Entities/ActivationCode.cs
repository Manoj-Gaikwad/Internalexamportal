using InternalExamportal.DataAccessLayer.Contracts;
using System.ComponentModel.DataAnnotations;

namespace InternalExamportal.DataAccessLayer.Entities
{
    public class ActivationCode : IAudited, ISoftDeleted
    {
        [Key]
        public int Id { get; set; }
        public int ActivationTypeId { get; set; }
        public ActivationType ActivationType { get; set; }
        public int TestId { get; set; }
        public Test Test { get; set; }
        public CommonCode CommonCode { get; set; }
        public AccessCode AccessCode { get; set; }

    }
}
