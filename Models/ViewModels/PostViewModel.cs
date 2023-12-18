using SimpleBlog.Models.Post;

namespace SimpleBlog.Models.ViewModels
{
    public class PostViewModel
    {
        private List<PostModel> _postList;
        private PostModel _viewablePost;

        public List<PostModel> PostList { get => _postList; set => _postList = value; }
        public PostModel ViewablePost { get => _viewablePost; set => _viewablePost = value; }


    }
}
