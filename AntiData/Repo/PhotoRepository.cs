using AntiData.Data;
using AntiData.Model;
using Microsoft.EntityFrameworkCore;

namespace AntiData.Repo;

public class PhotoRepository : IPhotoRepository
{
    private readonly MediaContext _context;

    public PhotoRepository(MediaContext context)
    {
        _context = context;
    }

    public IEnumerable<UserPhoto> FindAll()
    {
        return _context.Photos.OrderBy(p => p.Timestamp);
    }

    public UserPhoto FindById(int id)
    {
        return _context.Photos
            .First(photo => photo.Id == id);
    }

    public UserPhoto Create(UserPhoto entity)
    {
        var photo = _context.Photos.Add(entity).Entity;
        _context.SaveChanges();
        return photo;
    }

    public UserPhoto Update(UserPhoto entity)
    {
        var photo = FindById(entity.Id);
        photo.User = entity.User;
        photo.Url = entity.Url;
        _context.SaveChanges();
        return photo;
    }

    public UserPhoto Delete(UserPhoto entity)
    {
        return _context.Photos.Remove(entity).Entity;
    }

    public UserPhoto DeleteById(int id)
    {
        return _context.Photos.Remove(
            FindById(id)
        ).Entity;
    }

    public IEnumerable<UserPhoto> FindByUser(string userId)
    {
        return _context.Photos
            .Where(photo => photo.User.Id.Equals(userId))
            .OrderBy(p => p.Timestamp);
    }
}