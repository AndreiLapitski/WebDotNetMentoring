using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NorthwindApp.DTO;

namespace DotNetClient
{
    class Program
    {
        private const string BaseUrl = "http://localhost:58918/";

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            using (HttpClient client = new HttpClient { BaseAddress = new Uri(BaseUrl) })
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                IEnumerable<CategoryDto> categories = new List<CategoryDto>();
                HttpResponseMessage response = await client.GetAsync("api/categories");
                if (response.IsSuccessStatusCode)
                {
                    categories = await response.Content.ReadAsAsync<IEnumerable<CategoryDto>>();
                }

                return categories;
            }
        }

        static async Task<IEnumerable<ProductDto>> GetProducts()
        {
            using (HttpClient client = new HttpClient {BaseAddress = new Uri(BaseUrl)})
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                IEnumerable<ProductDto> products = new List<ProductDto>();
                HttpResponseMessage response = await client.GetAsync("api/products");
                if (response.IsSuccessStatusCode)
                {
                    products = await response.Content.ReadAsAsync<IEnumerable<ProductDto>>();
                }

                return products;
            }
        }

        static async Task RunAsync()
        {
            try
            {
                IEnumerable<CategoryDto> categories = await GetCategories();
                string categoriesJsonString = JsonConvert.SerializeObject(categories);
                Console.WriteLine($"Categories: {categoriesJsonString}");

                IEnumerable<ProductDto> products = await GetProducts();
                string productsJsonString = JsonConvert.SerializeObject(products);
                Console.WriteLine($"Products: {productsJsonString}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
