namespace SimpleBlog.Data.Entities;

public class Post
{
    public int PostId { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    public virtual Account AccountOwner { get; set; }
    public virtual ICollection<PostLike>? PostLikes { get; set; }
}