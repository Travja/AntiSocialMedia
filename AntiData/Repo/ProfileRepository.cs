using AntiData.Data;
using AntiData.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AntiData.Repo;

public class ProfileRepository : IProfileRepository
{
    private readonly UserManager<AntiUser> _userManager;
    private readonly MediaContext _context;

    public ProfileRepository(UserManager<AntiUser> userManager, MediaContext context)
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
}