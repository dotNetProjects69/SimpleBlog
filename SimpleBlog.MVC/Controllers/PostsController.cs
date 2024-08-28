using Mapster;
using Microsoft.AspNetCore.Mvc;
using SimpleBlog.Common.DTOs;
using SimpleBlog.MVC.Extensions;
using SimpleBlog.MVC.Shared;
using SimpleBlog.MVC.Validation.ViewModels.Post;
using SimpleBlog.Services.Extensions;
using SimpleBlog.Services.Services.Abstract;
using static SimpleBlog.MVC.Shared.GlobalParams;

namespace SimpleBlog.MVC.Controllers
{
    public class PostsController : Controller
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHandler _sessionHandler;
        private readonly string _format;
        private readonly string _accountsData;
        private readonly IPostService _postService;
        private readonly IPostLikeService _postLikeService;
        private readonly ILikeService _likeService;
        private readonly IAccountService _accountService;

        public PostsController(ILogger<PostsController> logger,
            IConfiguration configuration,
            ISessionHandler sessionHandler,
            IPostService postService,
            IPostLikeService postLikeService,
            ILikeService likeService,
            IAccountService accountService)
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHandler = sessionHandler;
            _postService = postService;
            _postLikeService = postLikeService;
            _likeService = likeService;
            _accountService = accountService;
            _format = _configuration["DateTimeFormat"] ?? "";
            _accountsData = GetAccountsDataPath();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int accountId = _sessionHandler.GetOwnerId();
            List<PostDto> posts = await _postService.GetPostsByAccountId(accountId);
            List<PostModel> postModels = posts.ParseToViewModels();
            return View(postModels);
        }

        [HttpGet]
        public IActionResult NewPost()
        {
            return View();
        }

        public async Task<IActionResult> ViewPost(int id)
        {
            PostDto? postDto = await _postService.GetModelById(id);
            PostModel? post = postDto?.ParseToViewModel();

            if (post == null)
                return RedirectToAction("Index");

            int accountId = _sessionHandler.GetOwnerId();
            List<PostLikeDto> postLikes = await _postLikeService.GetAllPostLikeByPostId(id);
            List<LikeDto> likes = await _likeService.GetAllLikesByAccountOwnerId(accountId);
            post.LikesCount = postLikes.Count;
            post.IsLiked = postLikes.Any(pl => likes.Any(l => l.LikeId == pl.LikeId));

            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            PostDto? postDto = await _postService.GetModelById(id);
            
            if (postDto != null)
                postDto.CreatedAt = postDto.CreatedAt.ToLocalTime();
            
            UpdatingPostModel? updatingPostModel = postDto?.ParseToUpdViewModel();
            return updatingPostModel is not null
                ? View(updatingPostModel)
                : RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Add(CreatingPostModel post)
        {
            post.CreatedAt = post.UpdatedAt = DateTime.Now.ToUniversalTime();
            PostDto postDto = post.ParseToDto();
            int ownerId = _sessionHandler.GetOwnerId();
            postDto.Owner = await _accountService.GetModelById(ownerId) ??
                            throw new InvalidDataException($"Invalid Account ID - {ownerId}");
            await _postService.AddModel(postDto);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Update(UpdatingPostModel post)
        {
            post.UpdatedAt = DateTime.Now.ToUniversalTime();
            PostDto? postDtoFromDb = await _postService.GetModelById(post.Id);
            PostDto postDto = post.ParseToDto();
            
            if (postDtoFromDb != null) 
                postDto.CreatedAt = postDtoFromDb.CreatedAt;
            
            await _postService.UpdateModel(postDto);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            await _postService.DeleteModel(id);
            return RedirectToAction("Index");
        }
    }
}