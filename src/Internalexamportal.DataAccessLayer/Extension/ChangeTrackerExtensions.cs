using InternalExamportal.DataAccessLayer.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Internalexamportal.DataAccessLayer.Extension
{
  public static class ChangeTrackerExtensions
    {
     
        public static void SetShadowProperties(this ChangeTracker changeTracker,int UserId)
        {
            changeTracker.DetectChanges();

            var timestamp = DateTime.UtcNow;

            foreach (var entry in changeTracker.Entries())
            {
                if (entry.Entity is IAudited)
                {
                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    {
                        entry.Property("ModifiedOn").CurrentValue = timestamp;
                        entry.Property("ModifiedById").CurrentValue = UserId;
                    }

                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("CreatedOn").CurrentValue = timestamp;
                        entry.Property("CreatedById").CurrentValue = UserId;
                        
                    }
                }

                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Property("IsDeleted").CurrentValue = true;
                }
            }
        }
    }
}
