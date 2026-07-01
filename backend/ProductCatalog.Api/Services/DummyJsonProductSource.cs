using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Services;

public class DummyJsonProductSource : IProductSource
{
    private readonly HttpClient _httpClient;

    public DummyJsonProductSource(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // DummyJSON vraća proizvode umotane u objekt: { "products": [...], "total": 194 }
    // pa nam treba pomoćna klasa za taj omotač
    private class ProductsResponse
    {
        public List<Product> Products { get; set; } = new();
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<ProductsResponse>("products?limit=0");
        return response?.Products ?? new List<Product>();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"products/{id}");
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<Product>();
    }

    public async Task<List<Product>> SearchProductsAsync(string query)
    {
        var response = await _httpClient.GetFromJsonAsync<ProductsResponse>(
            $"products/search?q={Uri.EscapeDataString(query)}");
        return response?.Products ?? new List<Product>();
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        var categories = await _httpClient.GetFromJsonAsync<List<string>>("products/category-list");
        return categories ?? new List<string>();
    }
}