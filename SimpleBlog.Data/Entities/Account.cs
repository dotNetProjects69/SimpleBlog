using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

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
    
    public virtual ICollection<Post>? Posts { get; set; }
}