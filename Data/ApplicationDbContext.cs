using ExtracurricularActivitiesManagement.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ScheduledActivity> ScheduledActivities { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<UserRole> Roles { get; set; }

        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Activity>().Property(a => a.Id).IsRequired();
            modelBuilder.Entity<Activity>().Property(a => a.ActivityType).IsRequired();
            modelBuilder.Entity<Activity>().Property(a => a.Description).IsRequired();
            modelBuilder.Entity<Activity>().Property(a => a.PrimaryColour).IsRequired();
            modelBuilder.Entity<Activity>().Property(a => a.SecondaryColour).IsRequired();
            modelBuilder.Entity<Activity>().Property(a => a.ActivityPicture).HasDefaultValue("default-activity-picture.jpg");

            modelBuilder.Entity<Teacher>().Property(t => t.FirstName).IsRequired();
            modelBuilder.Entity<Teacher>().Property(t => t.LastName).IsRequired();
            modelBuilder.Entity<Teacher>().Property(t => t.Description).IsRequired();

            modelBuilder.Entity<ScheduledActivity>().Property(s => s.ActivityId).IsRequired();
            modelBuilder.Entity<ScheduledActivity>().Property(s => s.TeacherId).IsRequired();
            modelBuilder.Entity<ScheduledActivity>().Property(s => s.StartTime).IsRequired();
            modelBuilder.Entity<ScheduledActivity>().Property(s => s.EndTime).IsRequired();
            modelBuilder.Entity<ScheduledActivity>().Property(s => s.Capacity).IsRequired();

            modelBuilder.Entity<Booking>().HasIndex(b => new { b.UserId, b.ScheduledActivityId }).IsUnique();

            modelBuilder.Entity<ApplicationUser>().Property(u => u.FirstName).IsRequired();
            modelBuilder.Entity<ApplicationUser>().Property(u => u.LastName).IsRequired();
            modelBuilder.Entity<ApplicationUser>().Property(u => u.BirthDate).IsRequired();
            modelBuilder.Entity<ApplicationUser>().Property(u => u.Gender).IsRequired();

            modelBuilder.Entity<UserRole>()
                .HasData(new UserRole { Name = UserRole.ROLE_STUDENT, NormalizedName = UserRole.ROLE_STUDENT.ToUpper() },
                         new UserRole { Name = UserRole.ROLE_ADMIN, NormalizedName = UserRole.ROLE_ADMIN.ToUpper() });
        }
    }
}
