using AntiData.Model;

namespace AntiData.Repo;

public interface IPhotoRepository : IMediaRepository<UserPhoto, int>
{
    IEnumerable<UserPhoto> FindByUser(string userId);
}