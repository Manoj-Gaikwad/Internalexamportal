using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
