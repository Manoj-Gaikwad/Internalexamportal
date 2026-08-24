using Internalexamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
    public class ApplicationErrorConfiguration : IEntityTypeConfiguration<ApplicationError>
    {
        public void Configure(EntityTypeBuilder<ApplicationError> builder)
        {
            builder.HasKey(error => error.Id);
        }
    }
}
