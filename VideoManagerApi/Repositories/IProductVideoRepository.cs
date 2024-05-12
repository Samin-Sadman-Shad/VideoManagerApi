using VideoManagerApi.Models;

namespace VideoManagerApi.Repositories
{
    public interface IProductVideoRepository
    {
        Task<IEnumerable<Video>> GetAllVideosAsync(int id);
        /*Task<Video> GetVideoByIdAsync(int id);*/

        Task AddVideo(Video video);
        Task DeleteVideo(int id);
    }
}
