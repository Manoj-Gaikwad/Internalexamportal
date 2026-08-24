using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
   public class ActivationTypeConfiguration : IEntityTypeConfiguration<ActivationType>
    {
        public void Configure(EntityTypeBuilder<ActivationType> builder)
        {
            builder.HasData(
             
              new ActivationType { Id = 1, Type = "CommonType" },

              new ActivationType { Id = 2, Type = "AccessType" }

 
              );

        }
    }
}
