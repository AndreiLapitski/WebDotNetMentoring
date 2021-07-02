using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MockQueryable.Moq;
using Moq;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;
using NorthwindApp.Pages.Categories;
using Xunit;

namespace Tests.RazorPages
{
    public class CategoryPageTests
    {
        [Fact]
        public void IndexPage_Success()
        {
            // Arrange
            IndexModel categoryPageModel = GetCategoriesModelMockWith3Items();

            // Act
            IActionResult result = categoryPageModel.OnGet();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        private IndexModel GetCategoriesModelMockWith3Items()
        {
            Mock<IRepository<Category>> categoryRepositoryMock = new Mock<IRepository<Category>>();
            categoryRepositoryMock.Setup(r => r.GetAll()).Returns(Get3Categories);
            return new IndexModel(categoryRepositoryMock.Object);
        }

        private IQueryable<Category> Get3Categories()
        {
            return new List<Category>
            {
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Category-1",
                    Description = "Description-1"
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Category-2",
                    Description = "Description-2"
                },
                new Category
                {
                    CategoryId = 3,
                    CategoryName = "Category-3",
                    Description = "Description-3"
                }
            }.AsQueryable().BuildMock().Object;
        }
    }
}
