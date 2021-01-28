using System;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Data;
using Catalog.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    /// <summary>
    /// Catalog controller
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly CatalogContext _context;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(CatalogContext context, ILogger<CatalogController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Listing products and categories
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();

            return Ok(products);
        }

        /// <summary>
        /// Get product with product id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductWithId(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                _logger.LogError($"Product with id {id} not found.");
                return NotFound();
            }

            return Ok(product);
        }

        /// <summary>
        /// Get products by category
        /// </summary>
        [HttpGet("GetProductByCategory/{category}")]
        public async Task<IActionResult> GetProductsByCategory(string category)
        {
            var products = await _context.Products
                .Where(x => x.Category.Contains(category, StringComparison.InvariantCultureIgnoreCase))
                .ToListAsync();

            return Ok(products);
        }

        /// <summary>
        /// Create new product
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductWithId), new {Id = product.Id}, product);
        }

        /// <summary>
        /// Update a product
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                _logger.LogError($"Id {id} does not match product id {product.Id}");
                return BadRequest();
            }

            var productFromDb = await _context.Products.FindAsync(id);

            if (productFromDb is null)
            {
                _logger.LogError($"Product with id {id} not found.");
                return NotFound();
            }

            _context.Entry(productFromDb).CurrentValues.SetValues(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productFromDb = await _context.Products.FindAsync(id);

            if (productFromDb is null)
            {
                _logger.LogError($"Product with id {id} not found.");
                return NotFound();
            }

            _context.Products.Remove(productFromDb);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}