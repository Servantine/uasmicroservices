using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Text.Json;
using OrderServices.Models;

namespace OrderServices.Services
{
    public class CostumerService : ICostumerService
    {
        private readonly HttpClient _httpClient;

        public CostumerService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("https://localhost:5222");
        }


        public async Task<IEnumerable<Costumers>> GetAllCostumers()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            using (var client = new HttpClient(handler))
            {
                var response = await _httpClient.GetAsync("/costumers");
                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadAsStringAsync();
                    var customers = JsonSerializer.Deserialize<IEnumerable<Costumers>>(results);
                    return customers;
                }
                else
                {
                    throw new ArgumentException($"Cannot get Costumers - httpstatus : {response.StatusCode}");
                }
            }
        }

        public async Task<Costumers> GetByCostumerId(int CustomerId)
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            using (var client = new HttpClient(handler))
            {
                var response = await client.GetAsync($"https://localhost:5222/costumers/{CustomerId}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<Costumers>();
                    if (result == null)
                    {
                        throw new ArgumentException($"Costumer with id {CustomerId} not found");
                    }
                    else
                    {
                        return result;
                    }
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    throw new ArgumentException($"Costumer with id {CustomerId} not found");
                }
                else
                {
                    throw new ArgumentException($"Cannot get Costumers - httpstatus : {response.StatusCode}");
                }
            }
        }
    }

    public interface ICostumerService
    {
        Task<IEnumerable<Costumers>> GetAllCostumers();
        Task<Costumers> GetByCostumerId(int costumersId);
    }
}
