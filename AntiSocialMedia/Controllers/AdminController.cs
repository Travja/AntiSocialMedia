using System.Diagnostics;
using AntiData.Model;
using AntiData.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Models;

namespace SocialMedia.Controllers;

public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly UserManager<AntiUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IProfileRepository _profileRepo;

    public AdminController(
        ILogger<AdminController> logger,
        UserManager<AntiUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IProfileRepository profileRepo)
    {
        _logger = logger;
        _userManager = userManager;
        _roleManager = roleManager;
        _profileRepo = profileRepo;
    }

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Roles()
    {
        var userMap = await _profileRepo.GetUsersAndRoles();

        ViewBag.Roles = _roleManager.Roles.ToList();
        return View(userMap);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> CreateRole(string role)
    {
        if (await _roleManager.RoleExistsAsync(role)) return Redirect("/Admin/Roles");

        await _roleManager.CreateAsync(new IdentityRole(role));
        return Redirect("/Admin/Roles");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> SetRole(string id, string role)
    {
        var user = _userManager.Users.SingleOrDefault(u => u.UserName == id);
        if (user == null) return Redirect("/Admin/Roles");

        await _userManager.RemoveFromRolesAsync(user, _userManager.GetRolesAsync(user).Result);
        await _userManager.AddToRoleAsync(user, role);

        return Redirect("/Admin/Roles");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}