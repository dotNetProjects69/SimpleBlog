namespace SimpleBlog.Common.DTOs;

public class PostLikeDto
{
    public int PostLikeId { get; set; }
    public int PostId { get; set; }
    public int LikeId { get; set; }
    public int Order { get; set; }
}