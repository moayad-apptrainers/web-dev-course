namespace Session19_Api.Models;

// The Product entity. EF Core maps this class to a "Products" table,
// with Id as the primary key (EF recognises the "Id" name automatically).
// This is the SAME Product you built in Week 3 — now with a Description
// field so the web page has something to show under each item.
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
}
