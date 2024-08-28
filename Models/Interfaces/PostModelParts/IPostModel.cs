namespace SimpleBlog.Models.Interfaces.PostModelParts
{
    public interface IPostModel : ITitle
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Nickname { get; set; }
        public int Likes { get; set; }
        public bool IsLiked { get; set; }
        public IErrorModel Error { get; set; }
    }
}