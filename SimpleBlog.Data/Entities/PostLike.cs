namespace SimpleBlog.Data.Entities;

public class PostLike
{
    public int PostLikeId { get; set; }
    public int PostId { get; set; }
    public int LikeId { get; set; }
    public int Order { get; set; }
    
    public virtual Post Post { get; set; }
    public virtual Like Like { get; set; }
}