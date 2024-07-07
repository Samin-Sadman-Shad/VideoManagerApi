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
            var product = await _dbContext.Products.FindAsync(productId);
            return product;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _dbContext.Products.Entry(product).State = EntityState.Added;
            //_dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product updatedProduct)
        {
            //_dbContext.Entry(updatedProduct).State = EntityState.Modified;
            var entity = _dbContext.Products.Attach(updatedProduct);
            entity.State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return updatedProduct;
        }

        public async Task<Product> DeleteProductAsync(int productId)
        {
            var product = await _dbContext.Products.FindAsync(productId);
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            if (product != null)
            {

            }
            return product;
        }
    }
}
