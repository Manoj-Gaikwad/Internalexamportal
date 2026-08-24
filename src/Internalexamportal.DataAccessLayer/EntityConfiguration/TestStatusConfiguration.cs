using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
    public class TestStatusConfiguration : IEntityTypeConfiguration<TestStatus>
    {
        public void Configure(EntityTypeBuilder<TestStatus> builder)
        {
            builder.HasData(

              new TestStatus { Id = 1, Status = "In Progress" },

              new TestStatus { Id = 2, Status = "Submitted" },

              new TestStatus { Id = 3, Status = "Evaluated" }

              );

        }
    }
}
