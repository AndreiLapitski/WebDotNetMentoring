using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace TestsWithGeneratedClient
{
    public class ApiProducts
    {
        private const string BaseUrl = "https://localhost:44388/";

        [Fact]
        public async Task GetAll_Success()
        {
            // Arrange
            Client client = new Client(BaseUrl, new HttpClient());

            // Act
            ICollection<ProductDto> products = await client.ProductsAllAsync();

            // Assert
            Assert.IsType<List<ProductDto>>(products);
        }
    }
}
