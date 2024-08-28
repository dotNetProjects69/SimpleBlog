using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.Data.Entities;

public class Account
{
    public int AccountId { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Nickname { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    
    public ICollection<Post>? Posts { get; set; }
}