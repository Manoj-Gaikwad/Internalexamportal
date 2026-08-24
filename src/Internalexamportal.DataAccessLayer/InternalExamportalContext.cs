using Internalexamportal.DataAccessLayer.Entities;
using InternalExamportal.DataAccessLayer;
using InternalExamportal.DataAccessLayer.Entities;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using Internalexamportal.Common.Extensions;
using System.Threading;
using System.Threading.Tasks;

using System;
using InternalExamportal.DataAccessLayer.Contracts;
using InternalExamportal.DataAccessLayer.Extension;
using System.Linq;
using System.Linq.Expressions;

namespace Internalexamportal.DataAccessLayer
{
    public class InternalExamportalContext : IdentityDbContext<User,Role,string>
    {
        public DbSet<AccessCode> AccessCode { get; set; }
        public DbSet<ActivationCode> ActivationCode { get; set; }
        public DbSet<ApplicationError> ApplicationError { get; set; }
        public DbSet<CandidateGroup> CandidateGroup { get; set; }
        public DbSet<CandidateNumbering> CandidateNumbering { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<CommonCode> CommonCode { get; set; }
        public DbSet<CompletedTestDetails> CompletedTestDetails { get; set; }
        public DbSet<CorrectOption> CorrectOption { get; set; }
        public DbSet<DifficultLevel> DifficultLevel { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswer { get; set; }
        public DbSet<QuestionMark> QuestionMark { get; set; }
        public DbSet<QuestionOption> QuestionOption { get; set; }
        public DbSet<QuestionType> QuestionType { get; set; }
        public DbSet<Setting> Setting { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<SubjectTopic> SubjectTopic { get; set; }
        public DbSet<SubmittedTest> SubmittedTest { get; set; }
        public DbSet<SubmittedOption> SubmittedOption { get; set; }
        public DbSet<SubjectiveAnswer> SubjectiveAnswer { get; set; }
        public DbSet<Test> Test { get; set; }
        public DbSet<TestInstruction> TestInstruction { get; set; }
        public DbSet<TestPublish> TestPublish { get; set; }
        public DbSet<TestQuestion> TestQuestion { get; set; }
        public DbSet<TestSetting> TestSetting { get; set; }
        public DbSet<TestSettingType> TestSettingType { get; set; }
        public DbSet<TestStatus> TestStatus { get; set; }
        public DbSet<TestType> TestType { get; set; }
        public DbSet<UserPersonalDetail> UserPersonalDetail { get; set; }
        private IEntityConfigurator entityConfigurator;
        private IPrincipal principal { get; set; }

        public InternalExamportalContext(DbContextOptions<InternalExamportalContext> options, IEntityConfigurator _entityConfigurator, IPrincipal _principal )
            : base(options)
        {
            entityConfigurator = _entityConfigurator;
            principal = _principal;
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var user = principal.Identity.GetUserId();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAudited)
                {
                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    {
                        entry.Property("ModifiedOn").CurrentValue = DateTime.UtcNow;
                        
                        entry.Property("ModifiedById").CurrentValue = user;
                    }
                    if (entry.State == EntityState.Added)
                    {
                       
                        entry.Property("CreatedOn").CurrentValue = DateTime.UtcNow;
                        entry.Property("CreatedById").CurrentValue = user;

                    }
                }

                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Property("IsDeleted").CurrentValue = true;
                }
            }
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            var user = principal.Identity.GetUserId();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAudited)
                {
                    if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                    {
                        entry.Property("ModifiedOn").CurrentValue = DateTime.UtcNow;
                        //entry.Property("ModifiedOn").CurrentValue = DateTime.Now;
                        entry.Property("ModifiedById").CurrentValue = user;
                    }
                    if (entry.State == EntityState.Added)
                    {
                        entry.Property("CreatedOn").CurrentValue = DateTime.UtcNow;
                        //entry.Property("CreatedOn").CurrentValue = DateTime.Now;
                        entry.Property("CreatedById").CurrentValue = user;

                    }
                }

                if (entry.State == EntityState.Deleted && entry.Entity is ISoftDeleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Property("IsDeleted").CurrentValue = true;
                }
            }
            return (base.SaveChangesAsync(true, cancellationToken));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes()) 
            {
                if (typeof(ISoftDeleted).IsAssignableFrom(entityType.ClrType)) { 
                    // 1. Add the IsDeleted property
                    //entityType.GetOrAddProperty("IsDeleted", typeof(bool));
                    var property = entityType.FindProperty("IsDeleted") ?? entityType.AddProperty("IsDeleted", typeof(bool));
                    // 2. Create the query filter
                    var parameter = Expression.Parameter(entityType.ClrType);

                // EF.Property<bool>(post, "IsDeleted")
                var propertyMethodInfo = typeof(EF).GetMethod("Property").MakeGenericMethod(typeof(bool));
                var isDeletedProperty = Expression.Call(propertyMethodInfo, parameter, Expression.Constant("IsDeleted"));

                // EF.Property<bool>(post, "IsDeleted") == false
                BinaryExpression compareExpression = Expression.MakeBinary(ExpressionType.Equal, isDeletedProperty, Expression.Constant(false));

                // post => EF.Property<bool>(post, "IsDeleted") == false
                var lambda = Expression.Lambda(compareExpression, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            var UserId= principal.Identity.GetUserId();
            modelBuilder.ShadowProperties();
            entityConfigurator.Apply(modelBuilder);
            modelBuilder.Entity<ApplicationError>()
                .HasKey(appError => appError.Id);

            // Call OnModelCreating method of IdentityDbContext<User>
            base.OnModelCreating(modelBuilder);

            // Override/Update configurations from Identity 
            // User Table
            modelBuilder
                .Entity<User>()
                // over-write default names that is prefixed with "AspNet"
                .ToTable(nameof(User)); 

            modelBuilder.Entity<User>()
                .Property(user => user.Id)
                .HasMaxLength(50);

            // Role Table
            modelBuilder
                .Entity<Role>()
                // over-write default names that is prefixed with "AspNet"
                .ToTable("Role");
                
            modelBuilder
                .Entity<Role>()
                .Property(role => role.Id)
                // over-write default names that is prefixed with "AspNet"
                .HasMaxLength(50); 

            // UserClaim Table
            modelBuilder
                .Entity<IdentityUserClaim<string>>()
                // over-write default names that is prefixed with "AspNet"
                .ToTable("UserClaim"); 

            // UserRole Table
            modelBuilder
                .Entity<IdentityUserRole<string>>()
                // over-write default names that is prefixed with "AspNet"
                .ToTable("UserRole"); 

            modelBuilder
                .Entity<IdentityUserRole<string>>()
                .Property(userRole => userRole.RoleId)
                .HasMaxLength(50);

            // UserLogin Table
            modelBuilder
                .Entity<IdentityUserLogin<string>>()
                // over-write default names that is prefixed with "AspNet"
                .ToTable("UserLogin"); 

            modelBuilder
                .Entity<IdentityUserLogin<string>>()
                .Property(userlogin => userlogin.LoginProvider)
                .HasMaxLength(200);

            modelBuilder
                .Entity<IdentityUserLogin<string>>()
                .Property(userlogin => userlogin.ProviderKey)
                .HasMaxLength(200);

            // RoleClaim Table
            modelBuilder
                .Entity<IdentityRoleClaim<string>>()
                // over-write default names that is prefixed with "AspNet"
                .ToTable("RoleClaim");

            // UserToken Table
            modelBuilder
                .Entity<IdentityUserToken<string>>()
                // over-write default names that is prefixed with "AspNet"
                .ToTable("UserToken"); 

            modelBuilder
                .Entity<IdentityUserToken<string>>()
                .Property(token => token.UserId)
                .HasMaxLength(50);

            modelBuilder
                .Entity<IdentityUserToken<string>>()
                .Property(token => token.Name)
                .HasMaxLength(200);

            modelBuilder
                .Entity<IdentityUserToken<string>>()
                .Property(token => token.LoginProvider)
                .HasMaxLength(200);

            modelBuilder
                .Entity<Subject>()
                .HasKey(id => id.Id);

            modelBuilder
               .Entity<SubjectTopic>()
               .HasOne(s => s.Subject)
               .WithMany(s => s.SubjectTopic)
               .HasForeignKey(s => s.SubjectId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
               .Entity<QuestionOption>()
               .HasOne(s => s.Question)
               .WithMany(s => s.QuestionOption)
               .HasForeignKey(s => s.QuestionId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
               .Entity<TestQuestion>()
               .HasOne(s => s.Question)
               .WithMany(s => s.TestQuestion)
               .HasForeignKey(s => s.QuestionId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
               .Entity<Question>()
               .HasKey(id => id.Id);       

            modelBuilder
              .Entity<QuestionOption>()
               .HasKey(id => id.Id);

            modelBuilder
              .Entity<QuestionAnswer>()
              .HasKey(id => id.Id);
              

            modelBuilder
              .Entity<QuestionMark>()
              .HasKey(id => id.Id);

            modelBuilder
              .Entity<QuestionType>()
              .HasKey(id => id.Id);

            modelBuilder
             .Entity<DifficultLevel>()
             .HasKey(id => id.Id);

            modelBuilder
             .Entity<UserPersonalDetail>()
             .HasKey(id => id.Id);

            modelBuilder
              .Entity<Test>()
              .HasKey(id => id.Id);

            modelBuilder.Entity<Test>()
              .Property(p => p.LinkId).HasDefaultValueSql("NEWID()");

            modelBuilder
              .Entity<TestSetting>()
              .HasKey(id => id.Id);

            modelBuilder
              .Entity<Setting>()
              .HasKey(id => id.Id);

            modelBuilder.Entity<Setting>()
               .HasOne(s => s.Test)
               .WithMany(s => s.Setting)
               .HasForeignKey(s => s.TestId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
              .Entity<TestQuestion>()
              .HasKey(id => id.Id);

            modelBuilder
             .Entity<TestPublish>()
             .HasKey(id => id.Id);

            modelBuilder
              .Entity<ActivationCode>()
              .HasKey(id => id.Id);

            modelBuilder
            .Entity<AccessCode>()
            .HasKey(id => id.Id);

            modelBuilder
            .Entity<CommonCode>()
            .HasKey(id => id.Id);

            modelBuilder
            .Entity<SubmittedTest>()
            .HasKey(id => id.Id);

            modelBuilder
                .Entity<SubmittedOption>()
                .HasKey(id => id.Id);

            modelBuilder
            .Entity<CompletedTestDetails>()
            .HasKey(id => id.Id);
        }
    }
}
