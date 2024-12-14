using Microsoft.EntityFrameworkCore;
using FoodApp.Models; // Import your model classes here

namespace FoodApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Define your database tables here using DbSet<T>
        public required DbSet<FoodModel> Foods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure entity properties and mappings
            modelBuilder.Entity<FoodModel>()
                .Property(f => f.ImageUrl)
                .HasColumnName("image");
        }

    }
}
