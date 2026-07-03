using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Services;

public class DummyJsonProductSource : IProductSource
{
    private readonly HttpClient _httpClient;

    public DummyJsonProductSource(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // DummyJSON ne vraća golu listu nego objekt { "products": [...], "total": 194 },
    // pa nam treba klasa istog oblika da se JSON ispravno pretvori
    private class ProductsResponse
    {
        public List<Product> Products { get; set; } = new();
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        // limit=0 znači "vrati sve proizvode" (inače DummyJSON vrati samo 30)
        ProductsResponse? response =
            await _httpClient.GetFromJsonAsync<ProductsResponse>("products?limit=0");

        if (response == null)
        {
            return new List<Product>();
        }

        return response.Products;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        HttpResponseMessage response = await _httpClient.GetAsync("products/" + id);

        // Ako DummyJSON kaže da proizvod ne postoji (npr. 404), vraćamo null
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        Product? product = await response.Content.ReadFromJsonAsync<Product>();
        return product;
    }

    public async Task<List<Product>> SearchProductsAsync(string query)
    {
        // EscapeDataString sigurno kodira korisnikov unos za URL
        // (razmak postane %20, znak & ne razbije adresu itd.)
        string url = "products/search?q=" + Uri.EscapeDataString(query);

        ProductsResponse? response =
            await _httpClient.GetFromJsonAsync<ProductsResponse>(url);

        if (response == null)
        {
            return new List<Product>();
        }

        return response.Products;
    }

    public async Task<List<string>> GetCategoriesAsync()
    {
        List<string>? categories =
            await _httpClient.GetFromJsonAsync<List<string>>("products/category-list");

        if (categories == null)
        {
            return new List<string>();
        }

        return categories;
    }
}