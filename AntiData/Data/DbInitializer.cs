using AntiData.Model;

namespace AntiData.Data;

public class DbInitializer
{
    public static void Initialize(MediaContext ctx)
    {
        ctx.Database.EnsureCreated();

        SetupPosts(ctx);
    }

    private static void SetupPosts(MediaContext ctx)
    {
        if (ctx.Posts.Any()) return;

        // var posts = new MediaPost[]
        // {
        //     new()
        //     {
        //         PostText = "asdfa"
        //     }
        // };
        //
        // ctx.Posts.AddRange(posts);
        ctx.SaveChanges();
    }
}