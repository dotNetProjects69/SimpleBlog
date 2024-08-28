namespace SimpleBlog.Common.DTOs;

public class LikeDto
{
    public int LikeId { get; set; }
    public int AccountSenderId { get; set; }
    public int AccountReceiverId { get; set; }
    public int PostReceiverId { get; set; }
    public DateTime LikeReceivedTime { get; set; }
}