using System.Diagnostics;
using AntiData.Model;
using AntiData.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;

namespace SocialMedia.Controllers;

public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly UserManager<AntiUser> _userManager;
    private readonly IMediaRepository<MediaPost, int> _postRepo;

    public UserController(ILogger<UserController> logger, UserManager<AntiUser> userManager,
        IMediaRepository<MediaPost, int> postRepo)
    {
        _logger = logger;
        _userManager = userManager;
        _postRepo = postRepo;
    }

    [Authorize]
    public IActionResult Feed(string username)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.UserName == username);
        if (user == null)
        {
            return Redirect("/");
        }
        else
        {
            return View(user);
        }
    }

    [Authorize]
    [HttpPost]
    public IActionResult CreatePost(string username, MediaPost post)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.UserName == username);
        if (user == null) return Redirect("/");

        post.User = user;
        var poster = _userManager.Users.SingleOrDefault(u => User.Identity != null && u.UserName == User.Identity.Name);
        if (poster == null) return Redirect("/u/" + username);
        post.Poster = poster;
        _postRepo.Create(post);
        return Redirect("/u/" + username);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}