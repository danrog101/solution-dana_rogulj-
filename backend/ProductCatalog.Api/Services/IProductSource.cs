using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Services;

// Apstrakcija izvora proizvoda - omogućava dodavanje novih izvora
// (baza, file system...) bez mijenjanja ostatka aplikacije
public interface IProductSource
{
    Task<List<Product>> GetProductsAsync();
    Task<Product?> GetProductByIdAsync(int id);
    Task<List<Product>> SearchProductsAsync(string query);
    Task<List<string>> GetCategoriesAsync();
}