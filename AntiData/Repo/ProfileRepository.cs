using AntiData.Data;
using AntiData.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AntiData.Repo;

public class ProfileRepository : IProfileRepository
{
    private readonly UserManager<AntiUser> _userManager;
    private readonly MediaContext _context;

    public ProfileRepository(
        UserManager<AntiUser> userManager,
        MediaContext context
    )
    {
        _userManager = userManager;
        _context = context;
    }

    public IEnumerable<UserProfile> FindAll()
    {
        return _context.Profiles;
    }

    public UserProfile FindById(int id)
    {
        return _context.Profiles
            .First(photo => photo.Id == id);
    }

    public UserProfile Create(UserProfile entity)
    {
        var photo = _context.Profiles.Add(entity).Entity;
        _context.SaveChanges();
        return photo;
    }

    public UserProfile Update(UserProfile entity)
    {
        var profile = FindById(entity.Id);
        profile.Age = entity.Age;
        profile.Name = entity.Name;
        profile.FavoriteFood = entity.FavoriteFood;
        profile.FavoriteBook = entity.FavoriteBook;
        _context.SaveChanges();
        return profile;
    }

    public UserProfile Delete(UserProfile entity)
    {
        return _context.Profiles.Remove(entity).Entity;
    }

    public UserProfile DeleteById(int id)
    {
        return _context.Profiles.Remove(
            FindById(id)
        ).Entity;
    }

    public UserProfile FindByUser(string userId)
    {
        return _userManager.Users
            .Single(user => user.Id.Equals(userId)).Profile;
    }

    public IEnumerable<AntiUser> SearchUsers(string? query, int minAge, int maxAge)
    {
        query ??= "";
        query = query.ToLower();
        return _userManager.Users
            .Include(u => u.Profile)
            .Where(
                u =>
                    (u.UserName.ToLower().Contains(query) || u.Profile.Name.ToLower().Contains(query))
                    && u.Profile.Age >= minAge && u.Profile.Age <= maxAge
            );
    }

    public async Task<Dictionary<AntiUser, IList<string>>> GetUsersAndRoles()
    {
        var userMap = new Dictionary<AntiUser, IList<string>>();
        var users = _userManager.Users
            .Include(u => u.Profile)
            .ToList();
        foreach (var user in users)
        {
            userMap.Add(user, await _userManager.GetRolesAsync(user));
        }

        return userMap;
    }
}