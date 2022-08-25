using AntiData.Model;

namespace AntiData.Repo;

public interface IPostRepository : IMediaRepository<MediaPost, int>
{
    IEnumerable<MediaPost> FindByUser(string userId);
}