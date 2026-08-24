using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
    public class QuestionTypeConfiguration : IEntityTypeConfiguration<QuestionType>
    {
        public void Configure(EntityTypeBuilder<QuestionType> builder)

        {

            builder.HasData(

              new QuestionType { Id = 1, Type = "Multiple Choice Question" },

              new QuestionType { Id = 2, Type = "True/False" },

              new QuestionType { Id = 3, Type = "Subjective" }

              );

        }
    }
}
