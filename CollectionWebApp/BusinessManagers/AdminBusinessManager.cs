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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPostService postService;
        private readonly IUserService userService;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IPostBusinessManager postBusinessManager;



        public AdminBusinessManager(
             UserManager<ApplicationUser> userManager,
            IPostService postService,
            IUserService userService,
            IWebHostEnvironment webHostEnvironment)
        {
            this.userManager = userManager;
            this.postService = postService;
            this.userService = userService;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IndexVm> GetAdminDashboard(ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await userManager.GetUserAsync(claimsPrincipal);
            return new IndexVm
            {
                Posts = postService.GetPosts(applicationUser)
            };


        }

        public async Task<AboutViewModel> GetAboutViewModel(ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await userManager.GetUserAsync(claimsPrincipal);
            return new AboutViewModel
            {
                ApplicationUser = applicationUser,
                SubHeader = applicationUser.SubHeader,
                Content = applicationUser.AboutContent
            };
        }

        public async Task UpdateAbout(AboutViewModel aboutViewModel, ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await userManager.GetUserAsync(claimsPrincipal);

            applicationUser.SubHeader = aboutViewModel.SubHeader;
            applicationUser.AboutContent = aboutViewModel.Content;

            if (aboutViewModel.HeaderImage != null)
            {
                string webRootPath = webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\UserFiles\Users\{applicationUser.Id}\HeaderImage.jpg";

                EnsureFolder(pathToImage);

                using (var fileStream = new FileStream(pathToImage, FileMode.Create))
                {
                    await aboutViewModel.HeaderImage.CopyToAsync(fileStream);
                }
            }

            await userService.Update(applicationUser);
        }

        private void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName.Length > 0)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
        }

        // public async Task<AdminDashboardViewModel> GetAdminDashboardViewModel(bool showUnpublishedPosts)
        // {
        //    var publishedPosts = await postQuery.GetPublishedPosts();
        //    var unpublishedPosts = showUnpublishedPosts ? await posQuery.GetUnpublishedPosts() : null;
        //
        //    var viewModel = new AdminDashboardViewModel
        //    {
        //      PublishedPosts = publishedPosts,
        //     UnpublishedPosts = unpublishedPosts
        //  };

        //   return viewModel;
        // }
    }
}
