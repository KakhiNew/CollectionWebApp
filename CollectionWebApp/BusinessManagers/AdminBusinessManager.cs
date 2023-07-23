using CollectionWebApp.BusinessManagers.Interfaces;
using CollectionWebApp.Data.Models;
using CollectionWebApp.Models.AdminViewModel;
using CollectionWebApp.Services;
using CollectionWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CollectionWebApp.BusinessManagers
{
    public class AdminBusinessManager : IAdminBusinessManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPostService _postService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IPostBusinessManager _postBusinessManager;

        public AdminBusinessManager(
             UserManager<ApplicationUser> userManager,
            IPostService postService,
            IUserService userService,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _postService = postService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IndexVm> GetAdminDashboard(ClaimsPrincipal user)
        {
            bool isAdmin = user.IsInRole(ApplicationUserRole.Admin.ToString());
            ApplicationUser? applicationUser = await _userManager.GetUserAsync(user);

            IEnumerable<Post> posts = isAdmin
                ? _postService.GetAllPosts()
                : _postService.GetPostsByUser(applicationUser);

            return new IndexVm
            {
                Posts = posts
            };


        }

        public async Task<AboutViewModel> GetAboutViewModel(ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await _userManager.GetUserAsync(claimsPrincipal);
            return new AboutViewModel
            {
                ApplicationUser = applicationUser,
                SubHeader = applicationUser.SubHeader,
                Content = applicationUser.AboutContent
            };
        }

        public async Task UpdateAbout(AboutViewModel aboutViewModel, ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await _userManager.GetUserAsync(claimsPrincipal);

            applicationUser.SubHeader = aboutViewModel.SubHeader;
            applicationUser.AboutContent = aboutViewModel.Content;

            if (aboutViewModel.HeaderImage != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\UserFiles\Users\{applicationUser.Id}\HeaderImage.jpg";

                EnsureFolder(pathToImage);

                using (var fileStream = new FileStream(pathToImage, FileMode.Create))
                {
                    await aboutViewModel.HeaderImage.CopyToAsync(fileStream);
                }
            }

            await _userService.Update(applicationUser);
        }

        private void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName.Length > 0)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }

       //  public async Task<AdminDashboardViewModel> GetAdminDashboardViewModel(bool showUnpublishedPosts)
         //{
        //    var publishedPosts = await postBusinessManager.GetPublishedPosts();
        //    var unpublishedPosts = showUnpublishedPosts ? await posQuery.GetUnpublishedPosts() : null;
        
         //  var viewModel = new AdminDashboardViewModel
           // {
         //    PublishedPosts = publishedPosts,
         //   UnpublishedPosts = unpublishedPosts
       //  };

      //    return viewModel;
       // }
    }
}
