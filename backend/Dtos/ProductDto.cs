using System.ComponentModel.DataAnnotations;

namespace Session19_Api.Dtos;

// A DTO (Data Transfer Object) is the shape the API accepts from clients
// (in this project, from the web page's "Add product" form).
// It carries only the fields a client is allowed to send, and its
// data annotations are validated automatically thanks to [ApiController].
public class ProductDto
{
    [Required]
    public string Name { get; set; }

    [Range(0, 100000)]
    public double Price { get; set; }

    public string Description { get; set; }
}
