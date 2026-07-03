namespace ProductCatalog.Api.Models;


public class ProductListResponse
{
    public List<ProductListItem> Items { get; set; } = new();
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}