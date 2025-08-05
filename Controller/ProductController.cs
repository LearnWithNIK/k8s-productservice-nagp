using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Models;

namespace ProductService.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(AppDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products.Select(p => MapToDto(p)));
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            return Ok(MapToDto(product));
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Category = dto.Category,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            dto.Id = product.Id;
            dto.CreatedAt = product.CreatedAt;

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, dto);
        }

        // PUT: api/products/bulk
        [HttpPut("bulk")]
        public async Task<IActionResult> BulkUpdateProducts(List<Product> products)
        {
            if (products == null || products.Count == 0)
                return BadRequest("Product list cannot be empty.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Get all product IDs from request
            var ids = products.Select(p => p.Id).ToList();

            // Fetch existing products from DB
            var existingProducts = await _context.Products
                .Where(p => ids.Contains(p.Id))
                .ToListAsync();

            if (existingProducts.Count != ids.Count)
                return NotFound("One or more products were not found.");

            // Update existing records
            foreach (var existing in existingProducts)
            {
                var dto = products.First(p => p.Id == existing.Id);

                existing.Name = dto.Name;
                existing.Description = dto.Description;
                existing.Price = dto.Price;
                existing.Category = dto.Category;
                existing.CreatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Category = dto.Category;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private static Product MapToDto(Product product)
        {
            return new Product
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category,
                CreatedAt = product.CreatedAt
            };
        }
    }
}