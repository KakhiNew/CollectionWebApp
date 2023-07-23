using CollectionWebApp.Models.AdminViewModel;
using System.Security.Claims;

namespace CollectionWebApp.BusinessManagers.Interfaces
{
    public interface IAdminBusinessManager
    {
        Task<IndexVm> GetAdminDashboard(ClaimsPrincipal user);
        Task<AboutViewModel> GetAboutViewModel(ClaimsPrincipal user);
        Task UpdateAbout(AboutViewModel aboutViewModel, ClaimsPrincipal user);
       
    }
}
