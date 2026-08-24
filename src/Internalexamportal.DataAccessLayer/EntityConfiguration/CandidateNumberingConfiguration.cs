using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
    public class CandidateNumberingConfiguration : IEntityTypeConfiguration<CandidateNumbering>
    {
        public void Configure(EntityTypeBuilder<CandidateNumbering> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.RowVersion)
                .IsConcurrencyToken()
                .ValueGeneratedOnAddOrUpdate();

            builder.HasData(
              new CandidateNumbering
              {
                  Id = 1,
                  Count = 0
              });

        }
    }
}
