using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using Thesis.Models;
using static NuGet.Packaging.PackagingConstants;

namespace Thesis.database
{
    public class CoursesDBContext : DbContext
    {
        public CoursesDBContext()
        {

        }
        // Entities        
        public DbSet<MenuItem> menus { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=IBM\SQLEXPRESS;initial catalog=courses;trusted_connection=true;TrustServerCertificate=True");
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
            // ...
        }
    }
}