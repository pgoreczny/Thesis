using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Thesis.Models;

namespace Thesis.database
{
    public class CoursesDBContext : IdentityDbContext<IdentityUser>
    {
        public CoursesDBContext()
        {

        }

        public CoursesDBContext(DbContextOptions<CoursesDBContext> options): base(options)
        {
        }
        // Entities        
        public DbSet<MenuItem> menus { get; set; }

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
            base.OnModelCreating(modelBuilder);
        }
    }
}