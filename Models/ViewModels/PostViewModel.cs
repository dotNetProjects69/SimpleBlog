using SimpleBlog.Models.Interfaces.PostModelParts;
using SimpleBlog.Models.Post;

namespace SimpleBlog.Models.ViewModels
{
    public class PostViewModel
    {
        private IReadOnlyCollection<IPostModel> _postList;
        private IPostModel _viewablePost;

        public IReadOnlyCollection<IPostModel> PostList
        {
            get => _postList;
            set => _postList = value;
        }

        public IPostModel ViewablePost
        {
            get => _viewablePost;
            set => _viewablePost = value;
        }
    }
}