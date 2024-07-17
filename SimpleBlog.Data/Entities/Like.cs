namespace SimpleBlog.Data.Entities;

public class Like
{
    public int LikeId { get; set; }
    public int AccountSenderId { get; set; }
    public int AccountReceiverId { get; set; }
    public int PostReceiverId { get; set; }
    public DateTime LikeReceivedTime { get; set; }
    
    public virtual ICollection<PostLike>? PostLikes { get; set; }
}