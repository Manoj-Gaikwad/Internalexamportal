using InternalExamportal.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.EntityConfiguration
{
    public class TestSettingTypeConfiguration : IEntityTypeConfiguration<TestSettingType>
    {
        public void Configure(EntityTypeBuilder<TestSettingType> builder)
        {
            builder.HasData(

                new TestSettingType { Id = 1, Type = "Shuffling" },

                new TestSettingType { Id = 2, Type = "Display Result" },

                new TestSettingType { Id = 3, Type = "Multiple Attempt" },

                new TestSettingType { Id = 4, Type = "Window Minimise Warning" },

                new TestSettingType { Id = 5, Type = "Display Answer Sheet" }

              );

        }
    }
}
