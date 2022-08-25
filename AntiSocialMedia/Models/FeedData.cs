using AntiData.Model;

namespace SocialMedia.Models;

public class FeedData
{
    public AntiUser User { get; set; }
    public IEnumerable<MediaPost> Posts { get; set; }
}