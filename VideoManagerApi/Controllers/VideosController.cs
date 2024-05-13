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
        private readonly ILogger<VideosController> _logger;

        public VideosController(ProductVideoContext dbContext, CacheService cacheService, ILogger<VideoController> logger)
        {
            _dbContext = dbContext;
        }

        // GET: api/products/{productId}/videos
        [HttpGet]
        public async Task<IActionResult> GetVideos(int productId)
        {
            var cacheKey = GetCacheKey(productId);

            _logger.LogInformation($"Checking cache for product videos (ID: {productId}) - Cache key: {cacheKey}");

            var videos = await _cacheService.GetFromCache<IEnumerable<Video>>(cacheKey);

            if (videos == null)
            {
                _logger.LogInformation($"Videos not found in cache for product (ID: {productId})");

                var product = await _dbContext.Products.Include(p => p.Videos).FirstOrDefaultAsync(p => p.Id == productId);
                if (product == null)
                {
                    return NotFound();
                }

                videos = product.Videos;
                await _cacheService.SetInCache(cacheKey, videos, TimeSpan.FromMinutes(5));

                _logger.LogInformation($"Fetched videos from database and stored in cache for product (ID: {productId})");
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
                _logger.LogInformation($"Video uploading for product (ID: {productId})");
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
            _logger.LogInformation($"Video deleting for product (ID: {productId})");
            return NoContent();
        }

        private string GetCacheKey(int productId)
        {
            return _cacheService.GetCacheKey(productId);
        }
    }
}
