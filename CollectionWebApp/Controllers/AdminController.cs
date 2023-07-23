using CollectionWebApp.BusinessManagers;
using CollectionWebApp.BusinessManagers.Interfaces;
using CollectionWebApp.Data.Models;
using CollectionWebApp.Models.AdminViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CollectionWebApp.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminBusinessManager _adminBusinessManager;
        private readonly IPostBusinessManager _postBusinessManager;

        public AdminController(
            IAdminBusinessManager adminBusinessManager,
            IPostBusinessManager postBusinessManager
            )
        {
            _adminBusinessManager = adminBusinessManager;
            _postBusinessManager = postBusinessManager;

        }

       public async Task<IActionResult> Index()
       {
            return View(await _adminBusinessManager.GetAdminDashboard(User));
       }

        public async Task<IActionResult> About()
        {
            return View(await _adminBusinessManager.GetAboutViewModel(User));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAbout(AboutViewModel aboutViewModel)
        {
            await _adminBusinessManager.UpdateAbout(aboutViewModel, User);
            return RedirectToAction("About");
        }
        // [HttpPost]
       //  public async Task<IActionResult> Index()
       //  {
        //   bool isAdmin = User.IsInRole(ApplicationUserRole.Admin.ToString());

         //  var viewModel = await adminBusinessManager.GetAdminDashboardViewModel(isAdmin);

         // return View(viewModel);
       //  }

    }
}
