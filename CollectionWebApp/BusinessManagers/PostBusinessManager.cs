using CollectionWebApp.Authorization;
using CollectionWebApp.BusinessManagers.Interfaces;
using CollectionWebApp.Data.Models;
using CollectionWebApp.Models.HomeViewModel;
using CollectionWebApp.Models.PostViewModel;
using CollectionWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System.Security.Claims;

namespace CollectionWebApp.BusinessManagers
{
    public class PostBusinessManager : IPostBusinessManager
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostService postService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IAuthorizationService authorizationService;

        public PostBusinessManager(
            UserManager<ApplicationUser> userManager,
            IPostService posttService,
            IWebHostEnvironment webHostEnvironment,
            IAuthorizationService authorizationService)
        {
            this.userManager = userManager;
            this.postService = posttService;
            this.webHostEnvironment = webHostEnvironment;
            this.authorizationService = authorizationService;
        }

        public IndexVm GetIndexViewModel(string searchString, int? page, bool onlyPublished)
        {
            int pageSize = 20;
            int pageNumber = page ?? 1;

            var postsQuery = postService.GetPostsByTitle(searchString ?? string.Empty);
            if (onlyPublished)
            {
                postsQuery = postsQuery.Where(post => post.Published);
            }

            var posts = postsQuery.ToList();

            return new IndexVm
            {
                Posts = new StaticPagedList<Post>(posts.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, posts.Count()),
                SearchString = searchString,
                PageNumber = pageNumber
            };
        }
        public async Task<ActionResult<PostVm>> GetPostViewModel(int? id, ClaimsPrincipal claimsPrincipal)
        {
            if (id is null)
                return new BadRequestResult();

            var postId = id.Value;

            var post = postService.GetPost(postId);

            if (post is null)
                return new NotFoundResult();

            if (!post.Published)
            {
                var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Read);

                if (!authorizationResult.Succeeded) return DetermineActionResult(claimsPrincipal);
            }

            return new PostVm
            {
                Post = post
            };
        }

        public async Task<Post> CreatePost(CreateVm vm, ClaimsPrincipal claimsPrincipal)
        {
            Post post = vm.Post;

            post.Creator = await userManager.GetUserAsync(claimsPrincipal);
            post.CreatedOn = DateTime.UtcNow;
            post.UpdatedOn = DateTime.Now;

            post = await postService.Add(post);

            string webRootPath = webHostEnvironment.WebRootPath;
            string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.Id}\HeaderImage.jpg";

            EnsureFolder(pathToImage);

            if (vm.HeaderImage is not null)
            {
                using var fileStream = new FileStream(pathToImage, FileMode.Create);
                await vm.HeaderImage.CopyToAsync(fileStream);
            }

            return post;

        }
        public async Task<ActionResult<Comment>> CreateComment(PostVm postVm, ClaimsPrincipal claimsPrincipal)
        {
            if (postVm.Post is null || postVm.Post.Id == 0)
                return new BadRequestResult();

            var post = postService.GetPost(postVm.Post.Id);

            if (post is null)
                return new NotFoundResult();

            var comment = postVm.Comment;

            comment.Author = await userManager.GetUserAsync(claimsPrincipal);
            comment.Post = post;
            comment.CreatedOn = DateTime.Now;

            if (comment.Parent != null)
            {
                comment.Parent = postService.GetComment(comment.Parent.Id);
            }

            return await postService.Add(comment);
        }

        public async Task<ActionResult<EditVm>> UpdatePost(EditVm editViewModel, ClaimsPrincipal claimsPrincipal)
        {
            var post = postService.GetPost(editViewModel.Post.Id);

            if (post is null)
                return new NotFoundResult();

            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Update);

            if (!authorizationResult.Succeeded) return DetermineActionResult(claimsPrincipal);

            post.Published = editViewModel.Post.Published;
            post.Title = editViewModel.Post.Title;
            post.Content = editViewModel.Post.Content;
            post.UpdatedOn = DateTime.Now;

            if (editViewModel.HeaderImage != null)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.Id}\HeaderImage.jpg";

                EnsureFolder(pathToImage);

                using (var fileStream = new FileStream(pathToImage, FileMode.Create))
                {
                    await editViewModel.HeaderImage.CopyToAsync(fileStream);
                }
            }

            return new EditVm
            {
                Post = await postService.Update(post)
            };
        }



        public async Task<ActionResult<EditVm>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal)
        {
            if (id is null)
                return new BadRequestResult();

            var postId = id.Value;

            var post = postService.GetPost(postId);

            if (post is null)
                return new NotFoundResult();

            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Update);

            if (!authorizationResult.Succeeded) return DetermineActionResult(claimsPrincipal);

            return new EditVm
            {
                Post = post
            };
        }

        public async Task DeletePost(int postId, ClaimsPrincipal claimsPrincipal)
        {
            var post = postService.GetPost(postId);

            if (post is null)
                return;

            var authorizationResult = await authorizationService.AuthorizeAsync(claimsPrincipal, post, Operations.Delete);

            if (!authorizationResult.Succeeded)
                return;

            string webRootPath = webHostEnvironment.WebRootPath;
            string pathToImage = $@"{webRootPath}\UserFiles\Posts\{post.Id}\HeaderImage.jpg";
            if (File.Exists(pathToImage))
            {
                File.Delete(pathToImage);
            }

            await postService.Delete(post);
        }

    

    private ActionResult DetermineActionResult(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identity.IsAuthenticated)
                return new ForbidResult();
            else
                return new ChallengeResult();
        }

        private void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName.Length > 0)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }
    }
}
