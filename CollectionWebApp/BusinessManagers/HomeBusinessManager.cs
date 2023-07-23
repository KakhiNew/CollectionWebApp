using CollectionWebApp.BusinessManagers.Interfaces;
using CollectionWebApp.Data.Models;
using CollectionWebApp.Models.HomeViewModel;
using CollectionWebApp.Services;
using CollectionWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;

namespace CollectionWebApp.BusinessManagers
{
    public class HomeBusinessManager : IHomeBusinessManager
    {
        private readonly IPostService postService;
        private readonly IUserService userService;

        public HomeBusinessManager(
            IPostService postService,
            IUserService userService)
        {
            this.postService = postService;
            this.userService = userService;
        }

        public ActionResult<AuthorVm> GetAuthorViewModel(string authorId, string searchString, int? page)
        {
            if (authorId is null)
                return new BadRequestResult();

            var applicationUser = userService.Get(authorId);

            if (applicationUser is null)
                return new NotFoundResult();

            int pageSize = 20;
            int pageNumber = page ?? 1;

            var posts = postService.GetPostsByTitle(searchString ?? string.Empty)
                .Where(post => post.Published && post.Creator == applicationUser && post.Approved);

            return new AuthorVm
            {
                Author = applicationUser,
                Posts = new StaticPagedList<Post>(posts.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, posts.Count()),
                SearchString = searchString,
                PageNumber = pageNumber
            };
        }
    }
}
