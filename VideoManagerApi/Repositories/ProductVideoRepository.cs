using Microsoft.EntityFrameworkCore;
using VideoManagerApi.Data;
using VideoManagerApi.Models;

namespace VideoManagerApi.Repositories
{
    public class ProductVideoRepository 
    {
        private readonly ProductVideoContext _dbContext;

        public ProductVideoRepository(ProductVideoContext context)
        {
            _dbContext = context;
        }

        public async Task AddVideoAsync(int productId, Video video)
        {
            video.ProductId = productId;
            _dbContext.Videos.Add(video);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteVideoAsync(int videoId)
        {
            var video = await _dbContext.Videos.FindAsync(videoId);
            if(video != null)
            {
                _dbContext.Videos.Remove(video);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteVideoAsync(int productId, int videoId)
        {
            var product = await _dbContext.Products.Include(p => p.Videos).FirstOrDefaultAsync(p => p.Id == productId);
            if(product != null)
            {
                var video =  product?.Videos.Find(p => p.Id == videoId);
                if (video != null)
                {
                    _dbContext.Videos.Remove(video);
                    await _dbContext.SaveChangesAsync();
                }
            }
            
        }

        public async Task<IEnumerable<Video>> GetAllVideosAsync(int productId)
        {
            var product = await _dbContext.Products.Include(p => p.Videos).FirstOrDefaultAsync(p => p.Id == productId);
            return product?.Videos;
        }
    }
}
