using CollectionWebApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectionWebApp.Services.Interfaces
{
    public interface IUserService
    {
        ApplicationUser Get(string id);
        Task<ApplicationUser> Update(ApplicationUser applicationUser);
    }
}
