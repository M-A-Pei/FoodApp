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
        public DbSet<FoodModel> Foods { get; set; }
    }
}
