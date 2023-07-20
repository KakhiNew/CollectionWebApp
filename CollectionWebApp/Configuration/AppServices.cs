using CollectionWebApp.Authorization;
using CollectionWebApp.BusinessManagers;
using CollectionWebApp.BusinessManagers.Interfaces;
using CollectionWebApp.Data;
using CollectionWebApp.Data.Models;
using CollectionWebApp.Services;
using CollectionWebApp.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;

namespace CollectionWeb.Configuration
{
    public static class AppServices
    {
        public static void AddServices(this IServiceCollection serviceCollection, IConfiguration Configuration)
        {
            serviceCollection.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
            serviceCollection.AddControllersWithViews().AddRazorRuntimeCompilation();
            serviceCollection.AddRazorPages();

            serviceCollection.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

            serviceCollection.AddScoped<IPostBusinessManager, PostBusinessManager>();
            serviceCollection.AddScoped<IAdminBusinessManager, AdminBusinessManager>();
            serviceCollection.AddScoped<IHomeBusinessManager, HomeBusinessManager>();

            serviceCollection.AddScoped<IPostService, PostService>();
            serviceCollection.AddScoped<IUserService, UserService>();

            serviceCollection.AddTransient<IAuthorizationHandler, PostAuthorizationHandler>();
        }
    }
}