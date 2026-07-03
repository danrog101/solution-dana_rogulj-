using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Services;

public interface IProductSource
{
    Task<List<Product>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<List<Product>> SearchProductsAsync(string query);
    Task<List<string>> GetCategoriesAsync();
}