using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NorthwindApp.Models;
using NorthwindApp.Repositories;
using Xunit;
using Tests.Utilities;
using static Tests.Utilities.TestsHelper;

namespace Tests.Repositories
{
    public class CategoryRepositoryTests : TestsBase
    {
        [Fact]
        public async Task Create_Success()
        {
            // Arrange
            int expected = 1;
            int actual;
            Category category = new Category
            {
                CategoryName = "Category-4",
                Description = "Description-4"
            };

            // Act
            await using (NorthwindContext context = new NorthwindContext(GetTestOptions()))
            {
                CategoryRepository categoryRepository = new CategoryRepository(context);
                actual = await categoryRepository.CreateAsync(category);
            }

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public async Task Save_Success()
        {
            // Arrange
            int expected = 1;
            int actual;
            string description = "Description-4";
            string newDescription = "Edited";
            Category editedCategory;
            Category category = new Category
            {
                CategoryId = 4,
                CategoryName = "Category-4",
                Description = description
            };

            // Act
            await using (NorthwindContext context = new NorthwindContext(GetTestOptions()))
            {
                CategoryRepository categoryRepository = new CategoryRepository(context);
                await categoryRepository.CreateAsync(category);
                category.Description = newDescription;
                actual = await categoryRepository.SaveAsync();
                editedCategory = await categoryRepository.GetByIdAsync(category.CategoryId);
            }

            // Assert
            Assert.Equal(expected, actual);
            Assert.Equal(newDescription, editedCategory.Description);
        }

        [Fact]
        public async Task GetById_Success()
        {
            // Arrange
            int correctCategoryId = 1;
            Category category;

            // Act
            await using (NorthwindContext context = new NorthwindContext(GetTestOptions()))
            {
                CategoryRepository categoryRepository = new CategoryRepository(context);
                category = await categoryRepository.GetByIdAsync(correctCategoryId);
            }

            // Assert
            Assert.NotNull(category);
        }

        [Fact]
        public async Task GetAll_Success()
        {
            // Arrange
            int expected, actual;

            // Act
            await using (NorthwindContext context = new NorthwindContext(GetTestOptions()))
            {
                CategoryRepository categoryRepository = new CategoryRepository(context);
                List<Category> categories = await categoryRepository.GetAll().ToListAsync();

                expected = 3;
                actual = categories.Count;
            }

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
