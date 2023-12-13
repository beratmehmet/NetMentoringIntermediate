using Microsoft.EntityFrameworkCore;
using RESTArchitecture.Models.Categories;
using RESTArchitecture.Models.Items;

namespace RESTArchitecture.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Active Wear - Men" },
                new Category { Id = 2, Name = "Active Wear - Women" },
                new Category { Id = 3, Name = "Mineral Water" },
                new Category { Id = 4, Name = "Publications" },
                new Category { Id = 5, Name = "Supplements" });

            modelBuilder.Entity<Item>().HasData(
                new Item { Id = 1, CategoryId = 1, Name = "Grunge Skater Jeans",  Price = 68 },
                new Item { Id = 2, CategoryId = 1, Name = "Polo Shirt",  Price = 35 },
                new Item { Id = 3, CategoryId = 1, Name = "Skater Graphic T-Shirt",  Price = 33 },
                new Item { Id = 4, CategoryId = 1, Name = "Slicker Jacket",  Price = 125 },
                new Item { Id = 5, CategoryId = 1, Name = "Thermal Fleece Jacket", Price = 60 },
                new Item { Id = 6, CategoryId = 1, Name = "Unisex Thermal Vest", Price = 95, },
                new Item { Id = 7, CategoryId = 1, Name = "V-Neck Pullover", Price = 65, },
                new Item { Id = 8, CategoryId = 1, Name = "V-Neck Sweater", Price = 65, },
                new Item { Id = 9, CategoryId = 1, Name = "V-Neck T-Shirt", Price = 17, },
                new Item { Id = 10, CategoryId = 2, Name = "Bamboo Thermal Ski Coat", Price = 99, },
                new Item { Id = 11, CategoryId = 2, Name = "Cross-Back Training Tank", Price = 0 },
                new Item { Id = 12, CategoryId = 2, Name = "Grunge Skater Jeans", Price = 68 },
                new Item { Id = 13, CategoryId = 2, Name = "Slicker Jacket", Price = 125 },
                new Item { Id = 14, CategoryId = 2, Name = "Stretchy Dance Pants", Price = 55 },
                new Item { Id = 15, CategoryId = 2, Name = "Ultra-Soft Tank Top", Price = 22 },
                new Item { Id = 16, CategoryId = 2, Name = "Unisex Thermal Vest", Price = 95 },
                new Item { Id = 17, CategoryId = 2, Name = "V-Next T-Shirt", Price = 17 },
                new Item { Id = 18, CategoryId = 3, Name = "Blueberry Mineral Water", Price = 2.8M },
                new Item { Id = 19, CategoryId = 3, Name = "Lemon-Lime Mineral Water", Price = 2.8M },
                new Item { Id = 20, CategoryId = 3, Name = "Orange Mineral Water", Price = 2.8M },
                new Item { Id = 21, CategoryId = 3, Name = "Peach Mineral Water", Price = 2.8M },
                new Item { Id = 22, CategoryId = 3, Name = "Raspberry Mineral Water", Price = 2.8M },
                new Item { Id = 23, CategoryId = 3, Name = "Strawberry Mineral Water", Price = 2.8M },
                new Item { Id = 24, CategoryId = 4, Name = "In the Kitchen with H+ Sport", Price = 24.99M },
                new Item { Id = 25, CategoryId = 5, Name = "Calcium 400 IU (150 tablets)", Price = 9.99M },
                new Item { Id = 26, CategoryId = 5, Name = "Flaxseed Oil 100 mg (90 capsules)", Price = 12.49M },
                new Item { Id = 27, CategoryId = 5, Name = "Iron 65 mg (150 caplets)", Price = 13.99M },
                new Item { Id = 28, CategoryId = 5, Name = "Magnesium 250 mg (100 tablets)", Price = 12.49M },
                new Item { Id = 29, CategoryId = 5, Name = "Multi-Vitamin (90 capsules)", Price = 9.99M },
                new Item { Id = 30, CategoryId = 5, Name = "Vitamin A 10,000 IU (125 caplets)", Price = 11.99M },
                new Item { Id = 31, CategoryId = 5, Name = "Vitamin B-Complex (100 caplets)", Price = 12.99M },
                new Item { Id = 32, CategoryId = 5, Name = "Vitamin C 1000 mg (100 tablets)", Price = 9.99M },
                new Item { Id = 33, CategoryId = 5, Name = "Vitamin D3 1000 IU (100 tablets)", Price = 12.49M });
        }
    }
}
