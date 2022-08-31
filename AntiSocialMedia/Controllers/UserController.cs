using System.Diagnostics;
using AntiData.Model;
using AntiData.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SocialMedia.Models;

namespace SocialMedia.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly UserManager<AntiUser> _userManager;
    private readonly IPostRepository _postRepo;
    private readonly IPhotoRepository _photoRepo;
    private readonly IProfileRepository _profileRepo;

    public UserController(
        ILogger<UserController> logger,
        UserManager<AntiUser> userManager,
        IPostRepository postRepo,
        IPhotoRepository photoRepo,
        IProfileRepository profileRepo
    )
    {
        _logger = logger;
        _userManager = userManager;
        _postRepo = postRepo;
        _photoRepo = photoRepo;
        _profileRepo = profileRepo;
    }

    [Authorize]
    public IActionResult Feed(string username)
    {
        var user = _userManager.Users.Include(u => u.Profile)
            .SingleOrDefault(u => u.UserName == username);
        if (user == null)
        {
            return Redirect("/");
        }

        var posts = _postRepo.FindByUser(user.Id);
        var photos = _photoRepo.FindByUser(user.Id);
        FeedData feedData = new()
        {
            User = user,
            Posts = posts,
            Photos = photos
        };
        return View(feedData);
    }

    [Authorize]
    [HttpPost]
    public IActionResult CreatePost(string username, string redirectUrl, MediaPost post)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.UserName == username);
        if (user == null) return Redirect("/");

        post.User = user;
        var poster = _userManager.Users.SingleOrDefault(u => User.Identity != null && u.UserName == User.Identity.Name);
        if (poster == null) return Redirect("/u/" + username);
        post.Poster = poster;
        _postRepo.Create(post);
        return Redirect(redirectUrl);
    }

    [Authorize]
    [HttpPost]
    public IActionResult CreatePhoto(string username, string redirectUrl, UserPhoto photo)
    {
        if (!username.Equals(User.Identity?.Name)) return Redirect(redirectUrl);

        var user = _userManager.Users.SingleOrDefault(u => u.UserName == username);
        if (user == null) return Redirect("/");

        photo.User = user;
        _photoRepo.Create(photo);
        return Redirect(redirectUrl);
    }

    [Authorize]
    public IActionResult ProfileEdit()
    {
        var username = User.Identity?.Name;
        var user = _userManager.Users
            .Include(u => u.Profile)
            .SingleOrDefault(u => u.UserName == username);
        if (user == null) return Redirect("/");

        return View(user.Profile);
    }

    [Authorize]
    [HttpPost]
    public IActionResult ProfileEdit(UserProfile profile)
    {
        if (!ModelState.IsValid) return View(profile);

        _profileRepo.Update(profile);

        return Redirect("/");
    }

    [Authorize]
    public IActionResult Search(string query, int minAge = 0, int maxAge = 130)
    {
        var users = _profileRepo.SearchUsers(query, minAge, maxAge);

        ViewBag.Query = query;
        ViewBag.MinAge = minAge;
        ViewBag.MaxAge = maxAge;
        
        return View(users);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}