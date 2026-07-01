namespace ProductCatalog.Api.Models;

// Skraćeni proizvod za listu - točno ono što zadatak traži:
// slika, naziv, cijena, opis do 100 znakova
public class ProductListItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Thumbnail { get; set; } = string.Empty;
}