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
        private readonly IAdminBusinessManager adminBusinessManager;
        private readonly IPostBusinessManager postBusinessManager;

        public AdminController(IAdminBusinessManager adminBusinessManager)
        {
            this.adminBusinessManager = adminBusinessManager;
            this.postBusinessManager = postBusinessManager;

        }

       public async Task<IActionResult> Index()
       {
            return View(await adminBusinessManager.GetAdminDashboard(User));
       }

        public async Task<IActionResult> About()
        {
            return View(await adminBusinessManager.GetAboutViewModel(User));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAbout(AboutViewModel aboutViewModel)
        {
            await adminBusinessManager.UpdateAbout(aboutViewModel, User);
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
