using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasData(

              new {
                  Id = 1,
                  Name = "Vidyaops Exam Portal",
                  Address = "Life Repulic R9-sector B1307 marunji-kasarsai Road Marunji , Pune 411057",
                  Phone = "9898989898",
                  Email = "manojdgaikwad4165@gmail.com",
                  IsDeleted = false
              }

           );

        }
    }
}
