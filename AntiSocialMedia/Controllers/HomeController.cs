using System.Diagnostics;
using AntiData.Model;
using AntiData.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;

namespace SocialMedia.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<AntiUser> _userManager;
    private readonly IPostRepository _postRepo;

    public HomeController(ILogger<HomeController> logger, UserManager<AntiUser> userManager, IPostRepository postRepo)
    {
        _logger = logger;
        _userManager = userManager;
        _postRepo = postRepo;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        if (User.Identity is not { IsAuthenticated: true, Name: { } }) return View();
        
        var user = _userManager.Users.SingleOrDefault(u => u.UserName == User.Identity.Name);
        if (user == null) return View();

        ViewBag.Posts = _postRepo.FindByUser(user.Id);

        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}