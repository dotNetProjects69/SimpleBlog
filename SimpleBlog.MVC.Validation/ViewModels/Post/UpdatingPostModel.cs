using System;
using SimpleBlog.MVC.Validation.ViewModels.Abstract;

namespace SimpleBlog.MVC.Validation.ViewModels.Post;

public class UpdatingPostModel : IHasId
{
    public int Id { get; set; }
    public string? Title { get; set; } = string.Empty;
    public string? Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}