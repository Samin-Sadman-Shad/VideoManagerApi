using Microsoft.EntityFrameworkCore;
using VideoManagerApi.Data;
using VideoManagerApi.Models;

namespace VideoManagerApi.Repositories
{
    public class ProductVideoRepository : IProductVideoRepository
    {
        private readonly ProductVideoContext _dbContext;

        public ProductVideoRepository(ProductVideoContext context)
        {
            _dbContext = context;
        }

        public async Task AddVideo(Video video)
        {
            _dbContext.Videos.Add(video);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteVideo(int id)
        {
            var video = await _dbContext.Videos.FirstOrDefaultAsync(x => x.VideoId == id);
            if(video != null)
            {
                _dbContext.Videos.Remove(video);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Video>> GetAllVideosAsync(int id)
        {
            return await _dbContext.Videos.ToListAsync();
        }
    }
}
