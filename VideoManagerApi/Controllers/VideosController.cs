using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoManagerApi.Cache;
using VideoManagerApi.Data;
using VideoManagerApi.Models;

namespace VideoManagerApi.Controllers
{
    [ApiController]
    [Route("api/products/{productId}/videos")]
    public class VideosController : ControllerBase
    {
        private readonly ProductVideoContext _dbContext;
        private readonly CacheService _cacheService;

        public VideosController(ProductVideoContext dbContext, CacheService cacheService)
        {
            _dbContext = dbContext;
        }

        // GET: api/products/{productId}/videos
        [HttpGet]
        public async Task<IActionResult> GetVideos(int productId)
        {
            var videos = await _cacheService.GetFromCache<IEnumerable<Video>>(GetCacheKey(productId));

            if (videos == null)
            {
                var product = await _dbContext.Products.Include(p => p.Videos).FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null)
                {
                    return NotFound();
                }

                videos = product.Videos;
                await _cacheService.SetInCache(GetCacheKey(productId), videos, TimeSpan.FromMinutes(5));
            }

            return Ok(videos);
        }

        // GET: api/products/{productId}/videos/{videoId}
        [HttpGet("{videoId}")]
        public async Task<IActionResult> GetVideo(int productId, int videoId)
        {
            var video = await _dbContext.Videos.FindAsync(videoId);
            if (video == null || video.ProductId != productId)
                return NotFound();

            return Ok(video);
        }

        // POST: api/products/{productId}/videos
        [HttpPost("{videoId}")]
        public async Task<IActionResult> UploadVideo(int productId, int videoId, Video video)
        {
            if (ModelState.IsValid)
            {
                video.ProductId = productId;
                _dbContext.Videos.Add(video);
                await _dbContext.SaveChangesAsync();
                return CreatedAtAction(nameof(GetVideo), new { productId, videoId }, video);
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/products/{productId}/videos/{videoId}
        [HttpDelete("{videoId}")]
        public async Task<IActionResult> DeleteVideo(int productId, int videoId)
        {
            var video = await _dbContext.Videos.FindAsync(videoId);
            if (video == null || video.ProductId != productId)
                return NotFound();

            _dbContext.Videos.Remove(video);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        private string GetCacheKey(int productId)
        {
            return _cacheService.GetCacheKey(productId);
        }
    }
}
