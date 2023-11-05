namespace SimpleBlog.Models
{
    public class PostModel
    {
        private int _id;
        private string _title = string.Empty;
        private string _body = string.Empty;
        private DateTime _createdAt;
        private DateTime _updatedAt;

        public int Id { get => _id; set => _id = value; }
        public string Title { get => _title; set => _title = value; }
        public string Body { get => _body; set => _body = value; }
        public DateTime CreatedAt { get => _createdAt; set => _createdAt = value; }
        public DateTime UpdatedAt { get => _updatedAt; set => _updatedAt = value; }
    }
}
