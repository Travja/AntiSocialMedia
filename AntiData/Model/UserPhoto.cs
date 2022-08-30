namespace AntiData.Model;

public class UserPhoto
{
    public int Id { get; set; }
    public string Url { get; set; }
    public AntiUser User { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
}