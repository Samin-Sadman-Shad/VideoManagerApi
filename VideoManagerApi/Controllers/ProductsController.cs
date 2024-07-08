using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoManagerApi.Data;
using VideoManagerApi.Models;

namespace VideoManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly ProductVideoContext _dbContext;
        private readonly ILogger<ProductsController> _logger;

        ProductsController(ProductVideoContext dbContext, ILogger<ProductsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }



        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _dbContext.Products.ToListAsync();
            _logger.LogInformation("product created");
            return Ok(products);
        }

        // GET: api/products/{productId}
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null)
            {
                _logger.LogWarning($"Product with id {productId} not found");
                return NotFound();
            }


            return Ok(product);
        }

        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetProduct), new { productId = product.Id }, product);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/products/{productId}
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, Product updatedProduct)
        {
            if (productId != updatedProduct.Id)
                return BadRequest();

            _dbContext.Entry(updatedProduct).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(productId))
                    return NotFound();
                throw;
            }
        }

        // DELETE: api/products/{productId}
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product == null)
                return NotFound();

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        private bool ProductExists(int productId)
        {
            return _dbContext.Products.Any(p => p.Id == productId);
        }
    }
}
