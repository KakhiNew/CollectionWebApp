using CollectionWebApp.Models.HomeViewModel;
using Microsoft.AspNetCore.Mvc;

namespace CollectionWebApp.BusinessManagers.Interfaces
{
    public interface IHomeBusinessManager
    {
        ActionResult<AuthorVm> GetAuthorViewModel(string authorId, string searchString, int? page);
    }
}
