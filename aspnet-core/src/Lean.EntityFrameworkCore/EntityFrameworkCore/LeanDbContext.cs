using Abp.IdentityServer4vNext;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Lean.Authorization.Delegation;
using Lean.Authorization.Roles;
using Lean.Authorization.Users;
using Lean.Chat;
using Lean.Editions;
using Lean.Friendships;
using Lean.MultiTenancy;
using Lean.MultiTenancy.Accounting;
using Lean.MultiTenancy.Payments;
using Lean.Storage;
using Lean.Lessons;
using Lean.UserLessonsProgress;

namespace Lean.EntityFrameworkCore
{
    public class LeanDbContext : AbpZeroDbContext<Tenant, Role, User, LeanDbContext>, IAbpPersistedGrantDbContext
    {
        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Module> Modules { get; set; }

        public virtual DbSet<Lesson> Lessons { get; set; }

        public virtual DbSet<Problem> Problems { get; set; }

        public virtual DbSet<Tag> Tags { get; set; }

        public virtual DbSet<ProblemTag> ProblemTags { get; set; }

        public virtual DbSet<ProblemAnswerOption> ProblemAnswerOptions { get; set; }

        public virtual DbSet<UserLearningProgress> UserLearningProgresses { get; set; }

        public virtual DbSet<UserLessonAnswerSet> UserLessonAnswerSets { get; set; }

        public virtual DbSet<UserProblemAnswerResult> UserProblemAnswerResults { get; set; }

        public virtual DbSet<UserProblemAnswerOptionResult> UserProblemAnswerOptionResults { get; set; }

        public virtual DbSet<UserTagRating> UserTagRatings { get; set; }


        public LeanDbContext(DbContextOptions<LeanDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.Entity<Course>(x =>
            {
                x.Property(e => e.Name).IsRequired();
                x.HasMany(e => e.Modules).WithOne(e => e.CourseFk).HasForeignKey(e => e.CourseId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Module>(x =>
            {
                x.Property(e => e.Name).IsRequired();
                x.HasMany(e => e.Lessons).WithOne(e => e.ModuleFk).HasForeignKey(e => e.ModuleId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Tag>(x =>
            {
                x.Property(e => e.Name).IsRequired();
                x.HasOne(e => e.ModuleFk).WithMany(e => e.Tags).HasForeignKey(e => e.ModuleId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Lesson>(x =>
            {
                x.Property(e => e.Name).IsRequired();
                x.HasMany(e => e.Problems).WithOne(e => e.LessonFk).HasForeignKey(e => e.LessonId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Problem>(x =>
            {
                x.HasIndex(e => new { e.LessonId, e.Number }).IsUnique();
                x.HasMany(e => e.ProblemAnswerOptions).WithOne(e => e.ProblemFk).HasForeignKey(e => e.ProblemId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProblemTag>(x =>
            {
                x.HasKey(e => new { e.ProblemId, e.TagId });
                x.HasOne(e => e.TagFk).WithMany(e => e.ProblemTags).HasForeignKey(e => e.TagId).OnDelete(DeleteBehavior.Cascade);
                x.HasOne(e => e.ProblemFk).WithMany(e => e.ProblemTags).HasForeignKey(e => e.ProblemId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ProblemAnswerOption>(x =>
            {
            });

            modelBuilder.Entity<UserLearningProgress>(x =>
            {
                x.HasOne(e => e.UserFk).WithOne(e => e.LearningProgressFk)
                    .HasForeignKey<UserLearningProgress>(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
                x.HasOne(e => e.CurrentLessonFk).WithMany()
                    .HasForeignKey(e => e.CurrentLessonId).OnDelete(DeleteBehavior.NoAction);
                x.HasOne(e => e.CurrentProblemFk).WithMany()
                    .HasForeignKey(e => e.CurrentProblemId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<UserLessonAnswerSet>(x =>
            {
                x.HasOne(e => e.UserFk).WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.NoAction);
                x.HasOne(e => e.LessonFk).WithMany().HasForeignKey(e => e.LessonId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserProblemAnswerResult>(x => 
            {
                x.HasOne(e => e.UserLessonAnswerSetFk).WithMany(e => e.Answers).HasForeignKey(e => e.UserLessonAnswerSetId)
                    .OnDelete(DeleteBehavior.Cascade);
                x.HasOne(e => e.ProblemFk).WithMany().HasForeignKey(e => e.ProblemId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<UserProblemAnswerOptionResult>(x =>
            {
                x.HasOne(e => e.ProblemAnswerOptionFk).WithMany().HasForeignKey(e => e.ProblemAnswerOptionId).OnDelete(DeleteBehavior.NoAction);
                x.HasOne(e => e.UserProblemResultFk).WithMany().HasForeignKey(e => e.UserProblemResultId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<UserTagRating>(x =>
            {
                x.Ignore(e => e.Id);
                x.HasKey(e => new { e.TagId, e.UserId });
                x.HasOne(e => e.UserFk).WithMany().HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.NoAction);
                x.HasOne(e => e.TagFk).WithMany().HasForeignKey(e => e.TagId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
