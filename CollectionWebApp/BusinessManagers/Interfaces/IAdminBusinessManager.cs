using CollectionWebApp.Models.AdminViewModel;
using System.Security.Claims;

namespace CollectionWebApp.BusinessManagers.Interfaces
{
    public interface IAdminBusinessManager
    {
        Task<IndexVm> GetAdminDashboard(ClaimsPrincipal claimsPrincipal);
        Task<AboutViewModel> GetAboutViewModel(ClaimsPrincipal claimsPrincipal);
        Task UpdateAbout(AboutViewModel aboutViewModel, ClaimsPrincipal claimsPrincipal);
       
    }
}
