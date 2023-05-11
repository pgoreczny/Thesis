using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Thesis.Models;
using File = Thesis.Models.File;
using Thesis.Areas.Forum.Models;

namespace Thesis.database
{
    public class CoursesDBContext : IdentityDbContext<ApplicationUser>
    {
        public CoursesDBContext()
        {

        }

        public CoursesDBContext(DbContextOptions<CoursesDBContext> options) : base(options)
        {
        }
        // Entities        
        public DbSet<MenuItem> menus { get; set; }
        public DbSet<Course> courses { get; set; }
        public DbSet<CourseApplicationUser> courseApplicationUsers { get; set; }
        public DbSet<Activity> activities { get; set; }
        public DbSet<File> files { get; set; }
        public DbSet<Answer> answers { get; set; }
        public DbSet<Currency> currencies { get; set; }
        public DbSet<ReviewComment> reviewComments { get; set; }
        public DbSet<Post> posts { get; set; }
        public DbSet<PostComment> postComments { get; set; }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(9, 2);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.Parent)
                    .WithMany(x => x.children)
                    .HasForeignKey(x => x.ParentId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.HasKey(x => x.id);
                entity.HasOne(x => x.course)
                    .WithMany(x => x.activities)
                    .HasForeignKey(x => x.courseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CourseApplicationUser>().HasKey(cau => new { cau.CourseId, cau.ApplicationUserId });

            modelBuilder.Entity<CourseApplicationUser>()
                .HasOne(au => au.applicationUser)
                .WithMany(s => s.CourseApplicationUsers)
                .HasForeignKey(c => c.ApplicationUserId);

            modelBuilder.Entity<PostComment>()
                .HasOne(comment => comment.post)
                .WithMany(post => post.comments)
                .HasForeignKey(comment => comment.postId);

            modelBuilder.Entity<Answer>()
                .HasMany(answer => answer.comments)
                .WithOne(comment => comment.answer)
                .HasForeignKey(comment => comment.AnswerId);

            modelBuilder.Entity<CourseApplicationUser>()
                .HasOne(c => c.course)
                .WithMany(c => c.CourseApplicationUsers)
                .HasForeignKey(au => au.CourseId);

            base.OnModelCreating(modelBuilder);
        }
    }
}