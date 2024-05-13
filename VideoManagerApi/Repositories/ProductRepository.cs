using Microsoft.EntityFrameworkCore;
using VideoManagerApi.Data;
using VideoManagerApi.Models;

namespace VideoManagerApi.Repositories
{
    public class ProductRepository
    {
        private readonly ProductVideoContext _dbContext;

        public ProductRepository(ProductVideoContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return await _dbContext.Products.FindAsync(productId);
        }

        public async Task AddProductAsync(Product product)
        {
            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product updatedProduct)
        {
            _dbContext.Entry(updatedProduct).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            if (product != null)
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
