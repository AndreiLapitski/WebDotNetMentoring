using Microsoft.EntityFrameworkCore;
using NorthwindApp.Models;

namespace Tests.Utilities
{
    public static class TestsHelper
    {
        public const string DbName = "NorthwindTest";

        public static DbContextOptions<NorthwindContext> GetTestOptions()
        {
            return new DbContextOptionsBuilder<NorthwindContext>().UseInMemoryDatabase(DbName).Options;
        }

        public static void FillInInMemoryNorthwindContextWithTestData()
        {
            using NorthwindContext context = new NorthwindContext(GetTestOptions());
            context.Categories.AddRange(
                new Category
                {
                    CategoryName = "Category-1",
                    Description = "Description-1"
                },
                new Category
                {
                    CategoryName = "Category-2",
                    Description = "Description-2"
                },
                new Category
                {
                    CategoryName = "Category-3",
                    Description = "Description-3"
                });
            context.SaveChanges();
        }

        public static void DropTestDatabase()
        {
            using NorthwindContext context = new NorthwindContext(GetTestOptions());
            context.Database.EnsureDeleted();
        }
    }
}
