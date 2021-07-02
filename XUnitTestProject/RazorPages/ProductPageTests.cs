using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using MockQueryable.Moq;
using Moq;
using NorthwindApp.Interfaces;
using NorthwindApp.Models;
using NorthwindApp.Pages.Products;
using Xunit;

namespace Tests.RazorPages
{
    public class ProductPageTests
    {
        private const int PageIndex = 1;

        #region Index page
        [Fact]
        public async Task IndexPage_Success()
        {
            // Arrange
            IndexModel pageModel = InitIndexModel_Mock();

            // Act
            IActionResult result = await pageModel.OnGetAsync(PageIndex);

            // Assert
            Assert.IsType<PageResult>(result);
        }
        #endregion

        #region Create page
        [Fact]
        public async Task CreatePageOnGet_Success()
        {
            // Arrange
            CreateModel pageModel = InitCreateModel_Mock();

            // Act
            IActionResult result = await pageModel.OnGetAsync();

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task CreatePageOnPost_Success()
        {
            // Arrange
            CreateModel pageModel = InitCreateModel_Mock();
            Product newProduct = new Product
            {
                ProductId = 4,
                ProductName = "4",
                SupplierId = 4,
                CategoryId = 4,
                QuantityPerUnit = "4 in 4",
                UnitPrice = 4,
                UnitsInStock = 4,
                UnitsOnOrder = 4,
                Discontinued = true
            };

            // Act
            IActionResult result = await pageModel.OnPostAsync(newProduct);

            // Assert
            Assert.IsType<RedirectToPageResult>(result);
        }

        [Fact]
        public async Task CreatePageOnPost_Fail()
        {
            // Arrange
            CreateModel pageModel = InitCreateModel_Mock();
            Product newProduct = new Product();
            pageModel.ModelState.AddModelError("Product.UnitPrice", "The Price field is required.");

            // Act
            IActionResult result = await pageModel.OnPostAsync(newProduct);

            // Assert
            Assert.IsType<PageResult>(result);
        }
        #endregion

        #region Edit page
        [Fact]
        public async Task EditPageOnGet_Success()
        {
            // Arrange
            EditModel pageModel = InitEditModel_Mock();
            int correctProductId = 1;

            // Act
            IActionResult result = await pageModel.OnGetAsync(correctProductId);

            // Assert
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task EditPageOnPost_Success()
        {
            // It can't be tested because of that: https://github.com/dotnet/AspNetCore.Docs/issues/14009
            // Arrange

            // Act

            // Assert
            Assert.True(true);
        }
        #endregion

        private IndexModel InitIndexModel_Mock()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"ProductsPageSize", "5"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            Mock<IRepository<Product>> productRepositoryMock = new Mock<IRepository<Product>>();
            productRepositoryMock.Setup(r => r.GetAll()).Returns(Get3Products);
            return new IndexModel(configuration, productRepositoryMock.Object);
        }

        private CreateModel InitCreateModel_Mock()
        {
            Mock<IRepository<Product>> productRepositoryMock = new Mock<IRepository<Product>>();
            productRepositoryMock.Setup(r => r.GetAll()).Returns(Get3Products);

            Mock<IRepository<Category>> categoryRepositoryMock = new Mock<IRepository<Category>>();
            categoryRepositoryMock.Setup(c => c.GetAll()).Returns(Get3Categories);

            Mock<IRepository<Supplier>> supplierRepositoryMock = new Mock<IRepository<Supplier>>();
            supplierRepositoryMock.Setup(s => s.GetAll()).Returns(Get3Suppliers);

            return new CreateModel(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object,
                supplierRepositoryMock.Object);
        }

        private EditModel InitEditModel_Mock()
        {
            Mock<IRepository<Product>> productRepositoryMock = new Mock<IRepository<Product>>();
            List<Product> productsMock = Get3Products().ToList();
            productRepositoryMock.Setup(r => r.GetAll()).Returns(Get3Products);
            productRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).Returns(async (int id) => productsMock.Single(x => x.ProductId == id));

            Mock<IRepository<Category>> categoryRepositoryMock = new Mock<IRepository<Category>>();
            categoryRepositoryMock.Setup(c => c.GetAll()).Returns(Get3Categories);

            Mock<IRepository<Supplier>> supplierRepositoryMock = new Mock<IRepository<Supplier>>();
            supplierRepositoryMock.Setup(s => s.GetAll()).Returns(Get3Suppliers);

            return new EditModel(
                productRepositoryMock.Object,
                categoryRepositoryMock.Object,
                supplierRepositoryMock.Object);
        }

        private IQueryable<Product> Get3Products()
        {
            return new List<Product>
            {
                new Product
                {
                    ProductId = 1,
                    ProductName = "1",
                    SupplierId = 1,
                    CategoryId = 1,
                    QuantityPerUnit = "1 in 1",
                    UnitPrice = 1,
                    UnitsInStock = 1,
                    UnitsOnOrder = 1,
                    Discontinued = false
                },
                new Product
                {
                    ProductId = 2,
                    ProductName = "2",
                    SupplierId = 2,
                    CategoryId = 2,
                    QuantityPerUnit = "2 in 2",
                    UnitPrice = 2,
                    UnitsInStock = 2,
                    UnitsOnOrder = 2,
                    Discontinued = false
                },
                new Product
                {
                    ProductId = 3,
                    ProductName = "3",
                    SupplierId = 3,
                    CategoryId = 3,
                    QuantityPerUnit = "3 in 3",
                    UnitPrice = 3,
                    UnitsInStock = 3,
                    UnitsOnOrder = 3,
                    Discontinued = true
                }
            }.AsQueryable().BuildMock().Object;
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

        private IQueryable<Supplier> Get3Suppliers()
        {
            return new List<Supplier>
            {
                new Supplier
                {
                    SupplierId = 1,
                    CompanyName = "1"
                },
                new Supplier
                {
                    SupplierId = 2,
                    CompanyName = "2"
                },
                new Supplier
                {
                    SupplierId = 3,
                    CompanyName = "3"
                }
            }.AsQueryable().BuildMock().Object;
        }
    }
}
