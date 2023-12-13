using Microsoft.EntityFrameworkCore;
using RESTArchitecture.Models.Categories;
using RESTArchitecture.Models.Items;

namespace RESTArchitecture.Models
{
    public class RestDbContext : DbContext
    {
        public RestDbContext(DbContextOptions<RestDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(c => c.Items)
                .WithOne(c => c.Category)
                .HasForeignKey(c => c.CategoryId);
            modelBuilder.Seed();
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
