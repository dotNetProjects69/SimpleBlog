using SimpleBlog.MVC.Validation.ViewModels.Abstract;

namespace SimpleBlog.MVC.Validation.ViewModels.PostLike;

public class UpdatingPostLikeModel : IHasId
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int LikeId { get; set; }
    public int Order { get; set; }
}