using AntiData.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AntiData.Data;

public class MediaContext : IdentityDbContext<AntiUser>
{
    public DbSet<MediaPost> Posts { get; set; }

    public MediaContext(DbContextOptions<MediaContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AntiUser>()
            .HasIndex(p => new { p.UserName })
            .IsUnique();
        modelBuilder.Entity<AntiUser>()
            .HasIndex(p => new { p.Email })
            .IsUnique();
    }
}