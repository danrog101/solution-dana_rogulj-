using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Models;
using ProductCatalog.Api.Services;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductSource _source;

    public ProductsController(IProductSource source)
    {
        _source = source;
    }

    [HttpGet]
    public async Task<ActionResult<ProductListResponse>> GetProducts(
        string? search,
        string? category,
        decimal? minPrice,
        decimal? maxPrice,
        int page = 1,
        int pageSize = 12)
    {
        
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 12;


        List<Product> allProducts;
        if (search == null || search.Trim() == "")
        {
            allProducts = await _source.GetProductsAsync();
        }
        else
        {
            allProducts = await _source.SearchProductsAsync(search);
        }


        List<Product> filtered = new List<Product>();
        foreach (Product p in allProducts)
        {
            if (category != null && category != "" &&
                !p.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                continue;

            if (minPrice != null && p.Price < minPrice)
                continue; 

            if (maxPrice != null && p.Price > maxPrice)
                continue; 

            filtered.Add(p);
        }


        int total = filtered.Count;
        int start = (page - 1) * pageSize; 

        List<ProductListItem> items = new List<ProductListItem>();
        for (int i = start; i < start + pageSize && i < total; i++)
        {
            items.Add(ToListItem(filtered[i]));
        }

    
        ProductListResponse response = new ProductListResponse
        {
            Items = items,
            Total = total,
            Page = page,
            PageSize = pageSize
        };

        return Ok(response);
    }

 
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        Product? product = await _source.GetProductByIdAsync(id);

        if (product == null)
            return NotFound(new { message = $"Proizvod s ID-em {id} ne postoji." });

        return Ok(product);
    }

   
    [HttpGet("categories")]
    public async Task<ActionResult<List<string>>> GetCategories()
    {
        List<string> categories = await _source.GetCategoriesAsync();
        return Ok(categories);
    }

    private static ProductListItem ToListItem(Product p)
    {
        string shortDesc = p.Description;
        if (shortDesc.Length > 100)
        {
            shortDesc = shortDesc.Substring(0, 100).TrimEnd() + "…";
        }

        return new ProductListItem
        {
            Id = p.Id,
            Title = p.Title,
            Price = p.Price,
            Thumbnail = p.Thumbnail,
            ShortDescription = shortDesc
        };
    }
}