using AntiData.Data;
using AntiData.Model;
using AntiData.Repo;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<MediaContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<AntiUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<MediaContext>();

// Add injection types
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    try
    {
        var context = services.GetRequiredService<MediaContext>();
        DbInitializer.Initialize(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occured creating the DB");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "edit",
    pattern: "about/edit",
    new { controller = "User", action = "ProfileEdit" }
);

app.MapControllerRoute(
    name: "user",
    pattern: "u/{username}",
    new { controller = "User", action = "Feed" }
);

app.MapControllerRoute(
    name: "search",
    pattern: "search",
    new { controller = "User", action = "Search" }
);

app.MapControllerRoute(
    name: "post",
    pattern: "post/{username}",
    new { controller = "User", action = "CreatePost" }
);

app.MapControllerRoute(
    name: "photo",
    pattern: "photo/{username}",
    new { controller = "User", action = "CreatePhoto" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();