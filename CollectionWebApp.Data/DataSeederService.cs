using CollectionWebApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CollectionWebApp.Data
{
    public interface IDataSeederService
    {
        Task MigrateAndSeed();
    }

    public class DataSeederService : IDataSeederService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DataSeederService(
            ApplicationDbContext dbContext,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager
            )
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task MigrateAndSeed()
        {
            await Migrate();
            await Seed();
        }

        private async Task Migrate()
        {
            await _dbContext.Database.MigrateAsync();
        }

        private async Task Seed()
        {
            await SeedRoles();
            await SeedUsers();
        }

        private async Task SeedRoles()
        {
            string[] roleNames = Enum.GetNames(typeof(ApplicationUserRole));

            foreach (string roleName in roleNames)
            {
                var roleExist = await _roleManager.RoleExistsAsync(roleName);
                if (roleExist)
                {
                    continue;
                }

                IdentityRole role = new(roleName);
                IdentityResult createRoleResult = await _roleManager.CreateAsync(role);
            }
        }

        private async Task SeedUsers()
        {
            string email = "admin@example.com";
            string password = "Kk.123456";
            ApplicationUser? adminUser = await _userManager.FindByEmailAsync(email);
            if (adminUser is not null)
            {
                return;
            }

            adminUser = new ApplicationUser()
            {
                Email = email,
                UserName = email,
                FirstName = "Admin",
                LastName = "User"
            };

            IdentityResult createResult = await _userManager.CreateAsync(adminUser, password);
            if (!createResult.Succeeded)
            {
                return;
            }

            await _userManager.AddToRoleAsync(adminUser, ApplicationUserRole.Admin.ToString());
        }
    }
}
