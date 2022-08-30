using AntiData.Data;
using AntiData.Model;
using Microsoft.EntityFrameworkCore;

namespace AntiData.Repo;

public class PostRepository : IPostRepository
{
    private readonly MediaContext _context;

    public PostRepository(MediaContext context)
    {
        _context = context;
    }

    public IEnumerable<MediaPost> FindAll()
    {
        return _context.Posts
            .Include(p => p.Poster.Profile)
            .OrderByDescending(p => p.Timestamp);
    }

    public MediaPost FindById(int id)
    {
        return _context.Posts
            .Include(p => p.Poster.Profile)
            .First(post => post.Id == id);
    }

    public MediaPost Create(MediaPost entity)
    {
        var post = _context.Posts.Add(entity).Entity;
        _context.SaveChanges();
        return post;
    }

    public MediaPost Update(MediaPost entity)
    {
        var post = FindById(entity.Id);
        post.Poster = entity.Poster;
        post.User = entity.User;
        post.PostText = entity.PostText;
        _context.SaveChanges();
        return post;
    }

    public MediaPost Delete(MediaPost entity)
    {
        return _context.Posts.Remove(entity).Entity;
    }

    public MediaPost DeleteById(int id)
    {
        return _context.Posts.Remove(
            FindById(id)
        ).Entity;
    }

    public IEnumerable<MediaPost> FindByUser(string userId)
    {
        return _context.Posts
            .Include(e => e.Poster)
            .Include(e => e.Poster.Profile)
            .Where(post => post.User.Id.Equals(userId))
            .OrderByDescending(p => p.Timestamp);
    }
}