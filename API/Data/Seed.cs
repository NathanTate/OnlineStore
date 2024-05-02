using API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static API.Utility.SD;

namespace API.Data
{
    public static class Seed
    {
        public static async Task SeedUsers(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserDataSeed.json");

            var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<ApplicationUser>>(userData, options);

            IEnumerable<UserRoles> enumRoles = (IEnumerable<UserRoles>)Enum.GetValues(typeof(UserRoles));

            var roles = new List<IdentityRole>(enumRoles.Select(role => new IdentityRole(role.ToString())));

            foreach(var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach(var user in users) 
            {
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, nameof(UserRoles.ADMIN));
            }
        }
    }
}
