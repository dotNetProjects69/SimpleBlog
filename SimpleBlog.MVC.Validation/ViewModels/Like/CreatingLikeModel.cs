using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleBlog.MVC.Validation.ViewModels.Like;

public class CreatingLikeModel
{
    [Required]
    public int AccountSenderId { get; set; }
    [Required]
    public int AccountReceiverId { get; set; }
    [Required]
    public int PostReceiverId { get; set; }
    [Required]
    public DateTime LikeReceivedTime { get; set; }
}