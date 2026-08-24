using Internalexamportal.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
    class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable(nameof(Role));

            builder.HasData(
                new Role
                {
                    Id = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6",
                    ConcurrencyStamp = "03f9632b-47fd-4ec4-a53f-39e14666d282",
                    Name = "SuperAdmin",
                    NormalizedName = "SuperAdmin".ToUpper(),
                    IsActive = true,
                    CreatedDate = DateTime.Parse("07/10/2020")
                },
                new Role
                {
                    Id = "859e583d-691f-40b4-adb2-828703ee1dd7",
                    ConcurrencyStamp = "31a59558-0a25-48f4-91ba-6a2ffc0ad41c",
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),
                    IsActive = true,
                    CreatedDate = DateTime.Parse("07/10/2020")
                },
                new Role
                {
                    Id = "15bda09a-effe-4634-88dc-855d9642bfed",
                    ConcurrencyStamp = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6",
                    Name = "Candidate",
                    NormalizedName = "Candidate".ToUpper(),
                    IsActive = true,
                    CreatedDate = DateTime.Parse("07/10/2020")
                }
            );
        }
    }
}
