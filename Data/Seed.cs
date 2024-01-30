using JetStoreAPI.Entities;
using JetStoreAPI.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace JetStoreAPI.Data
{
    public  class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if(await userManager.Users.AnyAsync())
            {
                return;
            }
            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions();
            options.Converters.Add(new DateOnlyJsonConverter());
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);
            if (users == null) { return; }
            var roles = new List<AppRole>
            {
                new AppRole{Name = "Employee"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Customer"}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.Email.ToLower();
                var result1 = await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Employee");
            }

            var admin = new AppUser
            {
                UserName = "admin",
                Name = "Gandalf",
                Paternal = "",
                Surname = "Grey",
                Email = "admin@mail.ru"
            };

           var result2 =  await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Employee" });
        }
    }
}

