namespace AntiData.Model;

public class MediaPost
{
    public int Id { get; set; }
    public AntiUser Poster { get; set; }
    public AntiUser User { get; set; }
    public string PostText { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}