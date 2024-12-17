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
        public required DbSet<UserModel> Users { get; set; }
    }
}
