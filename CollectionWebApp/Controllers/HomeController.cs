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
        private readonly IPostBusinessManager _postBusinessManager;
        private readonly IHomeBusinessManager _homeBusinessManager;
        private readonly IAdminBusinessManager _adminBusinessManager;

        public HomeController( 
            IPostBusinessManager postBusinessManager,
            IHomeBusinessManager homeBusinessManager,
            IAdminBusinessManager adminBusinessManager
            )
        {
            _postBusinessManager = postBusinessManager;
            _homeBusinessManager = homeBusinessManager;
            _adminBusinessManager = adminBusinessManager;

        }

        [Route("/")]
        public IActionResult Index(string searchString, int? page)
        {
             bool isAdmin = User.IsInRole(ApplicationUserRole.Admin.ToString()); 
             return View(_postBusinessManager.GetIndexViewModel(searchString, page, onlyPublished: !isAdmin));
            
        }

        
        public IActionResult Author(string authorId, string searchString, int? page)
        {
            var actionResult = _homeBusinessManager.GetAuthorViewModel(authorId, searchString, page);
            if (actionResult.Result is null)
                return View(actionResult.Value);

            return actionResult.Result;
        }

    }
}