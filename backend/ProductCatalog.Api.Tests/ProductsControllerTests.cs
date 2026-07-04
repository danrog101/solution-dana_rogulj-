using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using ProductCatalog.Api.Controllers;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Tests;

public class ProductsControllerTests
{
    private static Product MakeProduct(int id, string title, decimal price, string description)
    {
        Product p = new Product();
        p.Id = id;
        p.Title = title;
        p.Price = price;
        p.Description = description;
        p.Category = "test";
        return p;
    }

    private static ProductsController MakeController(FakeProductSource source)
    {
        return new ProductsController(source, NullLogger<ProductsController>.Instance);
    }

    [Fact]
    public async Task GetProducts_SkracujeOpisNa100Znakova()
    {
        FakeProductSource source = new FakeProductSource();
        string dugiOpis = new string('a', 150);
        source.ProductsToReturn.Add(MakeProduct(1, "Test", 10, dugiOpis));
        ProductsController controller = MakeController(source);

        ActionResult<ProductListResponse> result =
            await controller.GetProducts(null, null, null, null, 1, 12);

        OkObjectResult ok = (OkObjectResult)result.Result!;
        ProductListResponse response = (ProductListResponse)ok.Value!;

        Assert.Single(response.Items);
        Assert.True(response.Items[0].ShortDescription.Length <= 101);
        Assert.EndsWith("…", response.Items[0].ShortDescription);
    }

    [Fact]
    public async Task GetProducts_FiltriraPoRasponuCijene()
    {
        FakeProductSource source = new FakeProductSource();
        source.ProductsToReturn.Add(MakeProduct(1, "Jeftin", 50, "opis"));
        source.ProductsToReturn.Add(MakeProduct(2, "Srednji", 200, "opis"));
        source.ProductsToReturn.Add(MakeProduct(3, "Skup", 900, "opis"));
        ProductsController controller = MakeController(source);

        ActionResult<ProductListResponse> result =
            await controller.GetProducts(null, null, 100, 500, 1, 12);

        OkObjectResult ok = (OkObjectResult)result.Result!;
        ProductListResponse response = (ProductListResponse)ok.Value!;

        Assert.Single(response.Items);
        Assert.Equal("Srednji", response.Items[0].Title);
    }

    [Fact]
    public async Task GetProductById_VracaNotFoundZaNepostojeciId()
    {
        FakeProductSource source = new FakeProductSource();
        source.ProductsToReturn.Add(MakeProduct(1, "Jedini", 10, "opis"));
        ProductsController controller = MakeController(source);

        ActionResult<Product> result = await controller.GetProductById(99999);

        Assert.IsType<NotFoundObjectResult>(result.Result);
    }
}