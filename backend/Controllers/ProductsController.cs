using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;          // ToListAsync, FindAsync, SaveChangesAsync
using Session19_Api.Data;
using Session19_Api.Dtos;
using Session19_Api.Models;

namespace Session19_Api.Controllers;

// [ApiController] turns on helpful API behaviour (model binding, automatic 400s, etc.)
// [Route("api/[controller]")] -> the class "ProductsController" becomes "api/products"
// This is the SAME controller you wrote in Session 14 — the web page in the
// /frontend folder is now the client that calls these endpoints.
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // The database, handed to us by the DI container (constructor injection).
    private readonly AppDbContext _db;

    public ProductsController(AppDbContext db)
    {
        _db = db;
    }

    // GET api/products  ->  every product (200 OK)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _db.Products.ToListAsync();
        return Ok(products);
    }

    // GET api/products/1  ->  one product, or 404 if not found
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return NotFound();          // 404
        return Ok(product);             // 200
    }

    // POST api/products  ->  create a product from the JSON body (201 Created)
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            Description = dto.Description
        };

        _db.Products.Add(product);
        await _db.SaveChangesAsync();

        // 201 Created + a Location header pointing at the new item
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    // PUT api/products/1  ->  update an existing product (204 No Content)
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductDto dto)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        product.Name = dto.Name;
        product.Price = dto.Price;
        product.Description = dto.Description;

        await _db.SaveChangesAsync();
        return NoContent();             // 204
    }

    // DELETE api/products/1  ->  remove a product (204 No Content)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null)
            return NotFound();

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
