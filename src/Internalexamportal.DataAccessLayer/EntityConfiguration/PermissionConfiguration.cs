using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasData(
                new Permission { Id = 1, ParentId = 0, PermissionName = "Dashboard", Description = "", IsActive = true },
                new Permission { Id = 2, ParentId = 0, PermissionName = "Question Manager", Description = "", IsActive = true },
                new Permission { Id = 3, ParentId = 0, PermissionName = "Subject Manager", Description = "", IsActive = true },
                new Permission { Id = 4, ParentId = 0, PermissionName = "Test Manager", Description = "", IsActive = true },
                new Permission { Id = 5, ParentId = 0, PermissionName = "Candidates Manager", Description = "", IsActive = true },
                new Permission { Id = 6, ParentId = 0, PermissionName = "Admin Manager", Description = "", IsActive = true },

                //Dashboard
                new Permission { Id = 7, ParentId = 1, PermissionName = "View", Description = "", IsActive = true },
                new Permission { Id = 8, ParentId = 1, PermissionName = "Add", Description = "", IsActive = true },

                //Question
                new Permission { Id = 9, ParentId = 2, PermissionName = "View", Description = "", IsActive = true },
                new Permission { Id = 10, ParentId = 2, PermissionName = "Add/Edit", Description = "", IsActive = true },
                new Permission { Id = 11, ParentId = 2, PermissionName = "Delete", Description = "", IsActive = true },
                new Permission { Id = 12, ParentId = 2, PermissionName = "Import", Description = "", IsActive = true },

                //Subject
                new Permission { Id = 13, ParentId = 3, PermissionName = "View", Description = "", IsActive = true },
                new Permission { Id = 14, ParentId = 3, PermissionName = "Add/Edit", Description = "", IsActive = true },
                new Permission { Id = 15, ParentId = 3, PermissionName = "Delete", Description = "", IsActive = true },

                //Test
                new Permission { Id = 16, ParentId = 4, PermissionName = "View", Description = "", IsActive = true },
                new Permission { Id = 17, ParentId = 4, PermissionName = "Add/Edit", Description = "", IsActive = true },
                new Permission { Id = 18, ParentId = 4, PermissionName = "Delete", Description = "", IsActive = true },

                //Candidate
                new Permission { Id = 19, ParentId = 5, PermissionName = "View", Description = "", IsActive = true },
                new Permission { Id = 20, ParentId = 5, PermissionName = "Add/Edit", Description = "", IsActive = true },
                new Permission { Id = 21, ParentId = 5, PermissionName = "Delete", Description = "", IsActive = true },
                new Permission { Id = 22, ParentId = 5, PermissionName = "Import", Description = "", IsActive = true },

                //Admin
                new Permission { Id = 23, ParentId = 6, PermissionName = "View", Description = "", IsActive = true },
                new Permission { Id = 24, ParentId = 6, PermissionName = "Add/Edit", Description = "", IsActive = true },
                new Permission { Id = 25, ParentId = 6, PermissionName = "Delete", Description = "", IsActive = true },
                new Permission { Id = 26, ParentId = 6, PermissionName = "Role Permissions", Description = "", IsActive = true }
            );
        }
    }
}
