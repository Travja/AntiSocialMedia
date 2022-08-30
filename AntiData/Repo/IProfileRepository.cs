using AntiData.Model;

namespace AntiData.Repo;

public interface IProfileRepository : IMediaRepository<UserProfile, int>
{
    UserProfile FindByUser(string userId);
}