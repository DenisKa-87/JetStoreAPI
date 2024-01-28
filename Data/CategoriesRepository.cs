using JetStoreAPI.DTO;
using JetStoreAPI.Entities;
using JetStoreAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JetStoreAPI.Data
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly DataContext _context;

        public CategoriesRepository(DataContext context)
        {
            _context = context;
        }

        public void AddCategory(Category category)
        {
            _context.Add(category);
        }

        public void DeleteCategory(Category category)
        {
            _context.Remove(category);
        }

        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == name.ToLower());

        }

        public void UpdateCategory(Category oldCategory, CategoryDto newCategory)
        {
           oldCategory.Name = newCategory.Name;
           oldCategory.Description = newCategory.Description;
        }
    }
}
