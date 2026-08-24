using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Internalexamportal.DataAccessLayer.Entities
{
    public class Role : IdentityRole
    {
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
