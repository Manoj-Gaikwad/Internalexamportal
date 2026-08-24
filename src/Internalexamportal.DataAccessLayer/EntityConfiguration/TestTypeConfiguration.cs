using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
    public class TestTypeConfiguration : IEntityTypeConfiguration<TestType>
    {
        public void Configure(EntityTypeBuilder<TestType> builder)
        {

            builder.HasData(

              new TestType { Id = 1, Type = "Objective" },

              new TestType { Id = 2, Type = "Subjective" },

              new TestType { Id = 3, Type = "Objective & Subjective" }

              );

        }
    }
}
