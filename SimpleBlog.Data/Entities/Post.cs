namespace SimpleBlog.Data.Entities;

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public virtual Account Owner { get; set; }
    public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();
}