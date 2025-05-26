using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System;

namespace InventoryService.Clients
{
    public class ProductClient
    {
        private readonly HttpClient _httpClient;

        public ProductClient(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ProductService");
        }

        public async Task<ProductDto?> GetProductByIdAsync(int productId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/api/products/{productId}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                    return null;

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var jsonDoc = JsonDocument.Parse(content);
                var root = jsonDoc.RootElement.GetProperty("data").GetProperty("attributes");

                return new ProductDto
                {
                    Id = productId,
                    Nombre = root.GetProperty("nombre").GetString(),
                    Precio = root.GetProperty("precio").GetDecimal()
                };
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error al contactar ProductService: {e.Message}");
                return null;
            }
        }
    }

    public class ProductDto
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public decimal Precio { get; set; }
    }
}
