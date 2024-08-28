using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.MVC.Validation.ViewModels.Post;

public class PostModel
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int LikesCount { get; set; }
    public bool IsLiked { get; set; }
}