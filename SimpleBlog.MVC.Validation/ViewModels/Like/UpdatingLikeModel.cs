using System;
using SimpleBlog.MVC.Validation.ViewModels.Abstract;

namespace SimpleBlog.MVC.Validation.ViewModels.Like;

public class UpdatingLikeModel : IHasId
{
    public int Id { get; set; }
    public int AccountSenderId { get; set; }
    public int AccountReceiverId { get; set; }
    public int PostReceiverId { get; set; }
    public DateTime LikeReceivedTime { get; set; }
}