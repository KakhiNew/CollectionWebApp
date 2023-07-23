using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CollectionWebApp.Models;
using CollectionWebApp.BusinessManagers;
using CollectionWebApp.BusinessManagers.Interfaces;
using CollectionWebApp.Data.Models;

namespace CollectionWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostBusinessManager postBusinessManager;
        private readonly IHomeBusinessManager homeBusinessManager;
        private readonly IAdminBusinessManager adminBusinessManager;

        public HomeController( IPostBusinessManager postBusinessManager)
        {
            this.postBusinessManager = postBusinessManager;
            this.homeBusinessManager = homeBusinessManager;
            this.adminBusinessManager = adminBusinessManager;

        }

        [Route("/")]
        public IActionResult Index(string searchString, int? page)
        {
             bool isAdmin = User.IsInRole(ApplicationUserRole.Admin.ToString()); 
             return View(postBusinessManager.GetIndexViewModel(searchString, page, !isAdmin));
            
        }

        public IActionResult Author(string authorId, string searchString, int? page)
        {
            var actionResult = homeBusinessManager.GetAuthorViewModel(authorId, searchString, page);
            if (actionResult.Result is null)
                return View(actionResult.Value);

            return actionResult.Result;
        }

    }
}