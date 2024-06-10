using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using OrderServices.Models;

namespace OrderServices.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("https://localhost:7071");
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            using (var client = new HttpClient(handler))
            {
                var response = await _httpClient.GetAsync("/api/products");
                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();
                    var products = JsonSerializer.Deserialize<IEnumerable<Product>>(results);
                    return products;
                }
                else
                {
                    throw new ArgumentException($"Cannot get products - httpstatus : {response.StatusCode}");
                }
            }
        }

        public async Task<Product> GetByProductId(int productId)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            using (var client = new HttpClient(handler))
            {
                var response = await client.GetAsync($"https://localhost:7071/api/products/{productId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Product>();
                    if (result == null)
                    {
                        throw new ArgumentException($"Product with id {productId} not found");
                    }
                    else
                    {
                        return result;
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new ArgumentException($"Product with id {productId} not found");
                }
                else
                {
                    throw new ArgumentException($"Cannot get products - httpstatus : {response.StatusCode}");
                }
            }
        }
    }
}
