using Microsoft.AspNetCore.Mvc;
using CollectionWebApp.BusinessManagers.Interfaces;
using CollectionWebApp.BusinessManagers;
using CollectionWebApp.Models.PostViewModel;
using Microsoft.AspNetCore.Authorization;

namespace CollectionWebApp.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostBusinessManager postBusinessManager;

        public PostController(IPostBusinessManager postBusinessManager)
        {
            this.postBusinessManager = postBusinessManager;
        }

        [Route("Post/{id}"), AllowAnonymous]
        public async Task<IActionResult> Index(int? id)
        {
            var actionResult = await postBusinessManager.GetPostViewModel(id, User);

            if (actionResult.Result is null)
                return View(actionResult.Value);

            return actionResult.Result;
        }
        public IActionResult Create()
        {
            return View( new CreateVm());
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var actionResult = await postBusinessManager.GetEditViewModel(id, User);

            if (actionResult.Result is null)
                return View(actionResult.Value);

            return actionResult.Result;
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateVm createVm)
        {
            await postBusinessManager.CreatePost(createVm, User);
            return RedirectToAction("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditVm editVm)
        {
            var actionResult = await postBusinessManager.UpdatePost(editVm, User);

            if (actionResult.Result is null)
                return RedirectToAction("Edit", new { editVm.Post.Id });

            return actionResult.Result;
        }

        [HttpPost]
        public async Task<IActionResult> Comment(PostVm postVm)
        {
            var actionResult = await postBusinessManager.CreateComment(postVm, User);

            if (actionResult.Result is null)
                return RedirectToAction("Index", new { postVm.Post.Id });

            return actionResult.Result;
        }

    }
}
