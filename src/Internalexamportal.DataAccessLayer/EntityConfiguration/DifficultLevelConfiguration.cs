using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
  public  class DifficultLevelConfiguration : IEntityTypeConfiguration<DifficultLevel>
    {

        public void Configure(EntityTypeBuilder<DifficultLevel> builder)
        {
            builder.HasData(
           
              new DifficultLevel { Id = 1, Level = "Difficult" },

              new DifficultLevel { Id = 2, Level = "Easy" },

              new DifficultLevel { Id = 3, Level = "Normal" }

              );

        }
    }
}
