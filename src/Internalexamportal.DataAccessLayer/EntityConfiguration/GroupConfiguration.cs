using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
   public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasData(
             
              new Group { Id = 1, Name = "General", ClientId = 1 },

              new Group { Id = 2, Name = "Others", ClientId = 1 }

 
              );

        }
    }
}
