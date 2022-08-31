using AntiData.Model;
using Microsoft.AspNetCore.Identity;

namespace AntiData.Repo;

public interface IProfileRepository : IMediaRepository<UserProfile, int>
{
    UserProfile FindByUser(string userId);
    IEnumerable<AntiUser> SearchUsers(string query, int minAge, int maxAge);
    Task<Dictionary<AntiUser, IList<string>>> GetUsersAndRoles();
}