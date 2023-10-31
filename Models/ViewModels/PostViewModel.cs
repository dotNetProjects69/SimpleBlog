namespace SimpleBlog.Models.ViewModels
{
    public class PostViewModel
    {
        private List<PostModel> _postList;
        private PostModel _post;

        public List<PostModel> PostList { get => _postList; set => _postList = value; }
        public PostModel Post { get => _post; set => _post = value; }


    }
}
