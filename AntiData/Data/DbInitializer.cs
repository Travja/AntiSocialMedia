using AntiData.Model;
using Microsoft.AspNetCore.Identity;

namespace AntiData.Data;

public class DbInitializer
{
    public static async Task Initialize(MediaContext ctx, RoleManager<IdentityRole> roleManager)
    {
        ctx.Database.EnsureCreated();

        await SetupRoles(ctx, roleManager);
    }

    private static async Task SetupRoles(MediaContext ctx, RoleManager<IdentityRole> roleManager)
    {
        if (roleManager.Roles.Any()) return;

        await roleManager.CreateAsync(new IdentityRole("Admin"));
        await roleManager.CreateAsync(new IdentityRole("User"));
    }
}