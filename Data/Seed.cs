using JetStoreAPI.DTO;
using JetStoreAPI.Entities;
using JetStoreAPI.Helpers;
using JetStoreAPI.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SQLitePCL;
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

        internal static async void SeedItems(IUnitOfWork unitOfWork)
        {
            if (await unitOfWork.ItemsRepository.AnyAsync()) 
                return;
            var itemsData = await System.IO.File.ReadAllTextAsync("Data/ItemsSeedData.json");
            var itemsDto = JsonSerializer.Deserialize<List<ItemDto>>(itemsData);
            foreach (var itemDto in itemsDto)
            {
                var category = await unitOfWork.CategoriesRepository.GetCategoryById(itemDto.CategoryId);
                var unit = await unitOfWork.MeasureUnitsRepository.GetMeasureUnitById(itemDto.MeasureUnitId);
                if (category == null || unit == null)
                    return;
                var item = Item.CreateItem(itemDto, category, unit);
                unitOfWork.ItemsRepository.AddItem(item);
            }
            await unitOfWork.Complete();
        }

        internal static async void SeedUnits(IUnitOfWork unitOfWork)
        {
            if (await unitOfWork.MeasureUnitsRepository.AnyAsync())
                return;
            var data = await System.IO.File.ReadAllTextAsync("Data/UnitsSeedData.json");
            var units = JsonSerializer.Deserialize<List<MeasureUnit>>(data);
            if (units == null) { return; }
            foreach (var item in units)
            {
                unitOfWork.MeasureUnitsRepository.AddMeasureUnit(item);
            }
            await unitOfWork.Complete();
        }

        internal static async void SeedCategories(IUnitOfWork unitOfWork)
        {
            if (await unitOfWork.CategoriesRepository.AnyAsync())
                return;
            var data = await System.IO.File.ReadAllTextAsync("Data/CategoriesSeedData.json");
            var units = JsonSerializer.Deserialize<List<Category>>(data);
            if (units == null) { return; }
            foreach (var item in units)
            {
                unitOfWork.CategoriesRepository.AddCategory(item);
            }
            await unitOfWork.Complete();
        }
    }
}

