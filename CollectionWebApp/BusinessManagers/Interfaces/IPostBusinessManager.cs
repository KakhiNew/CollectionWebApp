using CollectionWebApp.Data.Models;
using CollectionWebApp.Models.HomeViewModel;
using CollectionWebApp.Models.PostViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CollectionWebApp.BusinessManagers.Interfaces
{
    public interface IPostBusinessManager
    {
        IndexVm GetIndexViewModel(string searchString, int? page, bool onlyPublished);
        Task<Post> CreatePost(CreateVm createVm, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<PostVm>> GetPostViewModel(int? id, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditVm>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<Comment>> CreateComment(PostVm postVm, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditVm>> UpdatePost(EditVm editViewModel, ClaimsPrincipal claimsPrincipal);
        Task DeletePost(int postId, ClaimsPrincipal claimsPrincipal);
       
    }
}
