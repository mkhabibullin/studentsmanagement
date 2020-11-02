using Microsoft.AspNetCore.Identity;
using SM.Application.Common.Enums;
using SM.Infrastructure.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace SM.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminRole = await roleManager.FindByNameAsync(Roles.Admin.ToString());
            if (adminRole == null)
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }

            var defaultUserEmail = "admin@email";
            var defaultUser = new ApplicationUser {
                UserName = defaultUserEmail,
                Email = defaultUserEmail,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "Admin1!");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                }
            }
        }
    }
}
