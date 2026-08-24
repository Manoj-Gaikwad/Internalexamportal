using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasData(
                //SuperAdmin
                new RolePermission { Id = 1, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 1 },
                new RolePermission { Id = 2, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 2 },
                new RolePermission { Id = 3, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 3 },
                new RolePermission { Id = 4, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 4 },
                new RolePermission { Id = 5, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 5 },
                new RolePermission { Id = 6, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 6 },
                new RolePermission { Id = 7, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 7 },
                new RolePermission { Id = 8, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 8 },
                new RolePermission { Id = 9, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 9 },
                new RolePermission { Id = 10, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 10 },
                new RolePermission { Id = 11, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 11 },
                new RolePermission { Id = 12, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 12 },
                new RolePermission { Id = 13, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 13 },
                new RolePermission { Id = 14, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 14 },
                new RolePermission { Id = 15, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 15 },
                new RolePermission { Id = 16, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 16 },
                new RolePermission { Id = 17, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 17 },
                new RolePermission { Id = 18, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 18 },
                new RolePermission { Id = 19, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 19 },
                new RolePermission { Id = 20, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 20 },
                new RolePermission { Id = 21, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 21 },
                new RolePermission { Id = 22, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 22 },
                new RolePermission { Id = 23, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 23 },
                new RolePermission { Id = 24, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 24 },
                new RolePermission { Id = 25, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 25 },
                new RolePermission { Id = 47, RoleId = "a7ffd1c1-c7ee-4ddc-a10e-882dcba329c6", PermissionId = 26 },

                //Admin
                new RolePermission { Id = 26, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 1 },
                new RolePermission { Id = 27, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 2 },
                new RolePermission { Id = 28, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 3 },
                new RolePermission { Id = 29, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 4 },
                new RolePermission { Id = 30, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 5 },
                new RolePermission { Id = 31, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 7 },
                new RolePermission { Id = 32, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 8 },
                new RolePermission { Id = 33, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 9 },
                new RolePermission { Id = 34, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 10 },
                new RolePermission { Id = 35, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 11 },
                new RolePermission { Id = 36, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 12 },
                new RolePermission { Id = 37, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 13 },
                new RolePermission { Id = 38, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 14 },
                new RolePermission { Id = 39, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 15 },
                new RolePermission { Id = 40, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 16 },
                new RolePermission { Id = 41, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 17 },
                new RolePermission { Id = 42, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 18 },
                new RolePermission { Id = 43, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 19 },
                new RolePermission { Id = 44, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 20 },
                new RolePermission { Id = 45, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 21 },
                new RolePermission { Id = 46, RoleId = "859e583d-691f-40b4-adb2-828703ee1dd7", PermissionId = 22 }
            );
        }
    }
}
