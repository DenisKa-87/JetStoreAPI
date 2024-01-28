using JetStoreAPI.DTO;
using JetStoreAPI.Entities;
using Microsoft.AspNetCore.Mvc;

namespace JetStoreAPI.Interfaces
{
    public interface ICategoriesRepository
    {
        Task<Category> GetCategoryById(int id);
        Task<Category> GetCategoryByName(string name);
        Task<ActionResult<IEnumerable<Category>>> GetCategories();
        void AddCategory(Category category);
        void UpdateCategory(Category oldCategory, CategoryDto newCategory);
        void DeleteCategory(Category category);
    }
}
