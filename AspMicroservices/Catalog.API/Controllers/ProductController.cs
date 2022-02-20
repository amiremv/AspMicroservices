using Catalog.API.Entities;
using Catalog.API.Entities.Dtos;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ILogger<ProductController> _logger;
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository repository, ILogger<ProductController> logger)
    {
        _productRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet(nameof(GetProducts))]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _productRepository.GetProducts();
        return Ok(products);
    }

    [HttpGet(nameof(GetProductById))]
    public async Task<ActionResult<Product>> GetProductById([FromQuery] string id)
    {
        var product = await _productRepository.GetProduct(id);
        return Ok(product);
    }

    [HttpGet(nameof(GetProductByName))]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByName([FromQuery] string name)
    {
        var products = await _productRepository.GetProductByName(name);
        return Ok(products);
    }

    [HttpGet(nameof(GetProductByCategory))]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory([FromQuery] string category)
    {
        var products = await _productRepository.GetProductByCategory(category);
        return Ok(products);
    }

    [HttpPost(nameof(Create))]
    public async Task<ActionResult<Product>> Create([FromBody] CreateProductDto dto)
    {
        var product = new Product();
        product.Id = new BsonObjectId(ObjectId.GenerateNewId()).ToString();
        product.Name = dto.Name;
        product.Category = dto.Category;
        product.Price = dto.Price;
        product.Summary = dto.Summary;
        product.Description = dto.Description;
        product.ImageFile = dto.ImageFile;

        var createdProduct = await _productRepository.Create(product);
        return Ok(createdProduct);
    }

    [HttpPut(nameof(Update))]
    public async Task<ActionResult<Product>> Update([FromBody] Product product)
    {
        var updatedProduct = await _productRepository.Update(product);
        return Ok(updatedProduct);
    }

    [HttpDelete(nameof(Update))]
    public async Task<ActionResult<Product>> Update([FromQuery] string id)
    {
        var deletedProduct = await _productRepository.Delete(id);
        return Ok(deletedProduct);
    }
}