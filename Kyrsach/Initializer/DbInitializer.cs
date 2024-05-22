
using Kyrsach.Models;
using Microsoft.AspNetCore.Identity;

namespace Kyrsach.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly SerovaContext _db;
        private readonly UserManager<UserModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(SerovaContext db, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Initialize()
        {
            string adminEmail = "admin@gmail.com";
            string password = "admin";
            string name = "Alina";
            if (await _roleManager.FindByNameAsync("admin") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await _roleManager.FindByNameAsync("user") == null)
            {
                await _roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await _userManager.FindByNameAsync(adminEmail) == null)
            {
                UserModel admin = new UserModel { Email = adminEmail, UserName = name };
                IdentityResult result = await _userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
