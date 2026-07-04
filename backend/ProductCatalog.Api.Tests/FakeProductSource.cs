using ProductCatalog.Api.Models;
using ProductCatalog.Api.Services;

namespace ProductCatalog.Api.Tests;

public class FakeProductSource : IProductSource
{
    public List<Product> ProductsToReturn { get; set; } = new List<Product>();

    public Task<List<Product>> GetProductsAsync()
    {
        return Task.FromResult(ProductsToReturn);
    }

    public Task<Product?> GetProductByIdAsync(int id)
    {
        Product? found = null;
        foreach (Product p in ProductsToReturn)
        {
            if (p.Id == id)
            {
                found = p;
            }
        }
        return Task.FromResult(found);
    }

    public Task<List<Product>> SearchProductsAsync(string query)
    {
        return Task.FromResult(ProductsToReturn);
    }

    public Task<List<string>> GetCategoriesAsync()
    {
        return Task.FromResult(new List<string>());
    }
}