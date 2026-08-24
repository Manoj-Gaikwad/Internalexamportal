using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Internalexamportal.DataAccessLayer.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Photo { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public string RollNumber { get; set; }

        public ICollection<CandidateGroup> CandidateGroup { get; set; }

        [NotMapped]
        public string FullName { get => FirstName + " " + LastName; }
    }
}
