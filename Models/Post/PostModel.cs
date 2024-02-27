using SimpleBlog.Models.Interfaces;
using SimpleBlog.Models.Interfaces.PostModelParts;

namespace SimpleBlog.Models.Post
{
    public class PostModel : IPostModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public int Likes { get; set; }
        public bool IsLiked { get; set; }
        public IErrorModel Error { get; set; } = new ErrorModel();
    }
}
