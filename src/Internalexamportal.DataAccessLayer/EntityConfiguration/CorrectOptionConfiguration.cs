using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
   public class CorrectOptionConfiguration : IEntityTypeConfiguration<CorrectOption>
    {
        public void Configure(EntityTypeBuilder<CorrectOption> builder)
        {
            builder.HasData(

              new CorrectOption { Id = 1, Option = "A" },

              new CorrectOption { Id = 2, Option = "B" },

              new CorrectOption { Id = 3, Option = "C" },

              new CorrectOption { Id = 4, Option = "D" },

              new CorrectOption { Id = 5, Option = "true" },

              new CorrectOption { Id = 6, Option = "false" },

              new CorrectOption { Id = 7, Option = "subjective" }
              );

        }
    }
}
