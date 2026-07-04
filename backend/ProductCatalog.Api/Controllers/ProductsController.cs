using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Api.Models;
using ProductCatalog.Api.Services;

namespace ProductCatalog.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductSource _source;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(IProductSource source, ILogger<ProductsController> logger)
    {
        _source = source;
        _logger = logger;
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

        _logger.LogInformation(
            "Dohvat proizvoda: search={Search}, category={Category}, minPrice={MinPrice}, maxPrice={MaxPrice}, page={Page}",
            search, category, minPrice, maxPrice, page);

        List<Product> allProducts;
        try
        {
            if (search == null || search.Trim() == "")
            {
                allProducts = await _source.GetProductsAsync();
            }
            else
            {
                allProducts = await _source.SearchProductsAsync(search);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Greška pri dohvatu proizvoda iz vanjskog izvora");
            return StatusCode(502, new { message = "Vanjski izvor podataka trenutno nije dostupan." });
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
        Product? product;
        try
        {
            product = await _source.GetProductByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Greška pri dohvatu proizvoda {Id} iz vanjskog izvora", id);
            return StatusCode(502, new { message = "Vanjski izvor podataka trenutno nije dostupan." });
        }

        if (product == null)
        {
            _logger.LogWarning("Traženi proizvod {Id} ne postoji", id);
            return NotFound(new { message = $"Proizvod s ID-em {id} ne postoji." });
        }

        return Ok(product);
    }

    [HttpGet("categories")]
    public async Task<ActionResult<List<string>>> GetCategories()
    {
        try
        {
            List<string> categories = await _source.GetCategoriesAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Greška pri dohvatu kategorija iz vanjskog izvora");
            return StatusCode(502, new { message = "Vanjski izvor podataka trenutno nije dostupan." });
        }
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