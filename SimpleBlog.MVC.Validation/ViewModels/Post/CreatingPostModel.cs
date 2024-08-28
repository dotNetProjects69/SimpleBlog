using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.MVC.Validation.ViewModels.Post;

public class CreatingPostModel
{
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}