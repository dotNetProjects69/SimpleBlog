using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.MVC.Validation.ViewModels.Post;

public class ViewableAccountPostModel
{
    public string AccountNickname { get; set; } = string.Empty;
    public int PostId { get; set; } = -1;
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public bool IsLiked { get; set; }
    public int LikesCount { get; set; }
}