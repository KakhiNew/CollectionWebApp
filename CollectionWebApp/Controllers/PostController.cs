using CollectionWebApp.Authorization;
using CollectionWebApp.BusinessManagers.Interfaces;
using CollectionWebApp.Models.PostViewModel;
using CollectionWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [Route("Post/Create")]
        public IActionResult Create()
        {
            return View(new CreateVm());
        }

        [Route("Post/Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            var actionResult = await postBusinessManager.GetEditViewModel(id, User);

            if (actionResult.Result is null)
                return View(actionResult.Value);

            return actionResult.Result;
        }
        [HttpPost]
        [Route("Post/Add")]
        public async Task<IActionResult> Add(CreateVm createVm)
        {
            await postBusinessManager.CreatePost(createVm, User);
            return RedirectToAction("Create");
        }

        [HttpPost]
        [Route("Post/Update")]
        public async Task<IActionResult> Update(EditVm editVm)
        {
            var actionResult = await postBusinessManager.UpdatePost(editVm, User);

            if (actionResult.Result is null)
                return RedirectToAction("Edit", new { editVm.Post.Id });

            return actionResult.Result;
        }

        [HttpPost]
        [Route("Post/Comment")]
        public async Task<IActionResult> Comment(PostVm postVm)
        {
            var actionResult = await postBusinessManager.CreateComment(postVm, User);

            if (actionResult.Result is null)
                return RedirectToAction("Index", new { postVm.Post.Id });

            return actionResult.Result;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var user = this.User;
            var post = await postBusinessManager.GetPostViewModel(id, user);

            if (post.Result is null)
                return NotFound();

            await postBusinessManager.DeletePost(id, user);

            return RedirectToAction(nameof(Index));
        }

    }
}
