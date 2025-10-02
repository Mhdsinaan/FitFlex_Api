using FitFlex.Application.DTO_s;
using FitFlex.Domain.Entities;
using FitFlex.Domain.Entities.Attendance;
using FitFlex.Domain.Entities.Session_model;
using FitFlex.Domain.Entities.stripePayment;
using FitFlex.Domain.Entities.Subscription_model;
using FitFlex.Domain.Entities.Trainer_model;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Domain.Entities.WorkoutPlan_Model;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitFlex.Infrastructure.Db_context
{
    public class MyContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MyContext(DbContextOptions<MyContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<SubscriptionPlan> SubscriptionPlans { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<UserTrainer> UserTrianers { get; set; }
        public DbSet<Workout> WorkoutPlans{ get; set; }
        public DbSet<WorkoutExercise> WorkoutExercise { get; set; }
        public DbSet<UserWorkoutAssignment> UserWorkoutAssignment { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<UserSession> UserSession { get; set; }
        public DbSet<Attendance> Attendances { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserTrainer>(entity =>
            {
                entity.HasKey(e => e.Id);

                
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();

              
                entity.HasOne(e => e.User)
                      .WithMany(u => u.UserTrainers)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Trainer)
                      .WithMany(t => t.UserTrainers)
                      .HasForeignKey(e => e.TrainerId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(e => e.WorkoutPlan)
                .WithMany(p => p.Exercises)
                .HasForeignKey(e => e.WorkoutPlanId);

            modelBuilder.Entity<UserWorkoutAssignment>()
                .HasOne(a => a.WorkoutPlan)
                .WithMany(p => p.Assignments)
                .HasForeignKey(a => a.WorkoutPlanId);

            

            //modelBuilder.Entity<UserSubscriptionAddOn>()
            //            .HasOne(a => a.UserSubscription)
            //            .WithMany(u => u.Ad)
            //            .HasForeignKey(a => a.UserSubscriptionId);
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedOn = DateTime.UtcNow;
                    entry.Entity.CreatedBy = string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId);
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedOn = DateTime.UtcNow;
                    entry.Entity.ModifiedBy = string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId);
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified; // soft delete
                    entry.Entity.DeletedOn = DateTime.UtcNow;
                    entry.Entity.DeletedBy = string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId);
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
