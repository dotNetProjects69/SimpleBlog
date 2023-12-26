using SimpleBlog.Models.Interfaces.PostModelParts;

namespace SimpleBlog.Models.Post
{
    public class PostModel : ITitle
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Nickname { get; set; } = string.Empty;
    }
}
