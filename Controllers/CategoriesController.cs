using JetStoreAPI.Data;
using JetStoreAPI.DTO;
using JetStoreAPI.Entities;
using JetStoreAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace JetStoreAPI.Controllers
{
    public class CategoriesController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _unitOfWork.CategoriesRepository.GetCategories();
        }

        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(CategoryDto categoryDto)
        {
            var category = await CategoryExistsAsync(categoryDto);
            if(category == null)
            {
                category = new Category()
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description
                };
                _unitOfWork.CategoriesRepository.AddCategory(category);
            }
            if(await _unitOfWork.Complete())
                return Ok(category);


            return BadRequest(new { message = "The category already exists." });

        }

        private async Task<Category> CategoryExistsAsync(CategoryDto categoryDto)
        {
            return await _unitOfWork.CategoriesRepository.GetCategoryByName(categoryDto.Name);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.CategoriesRepository.GetCategoryById(id);
            if(category == null)
            {
                return BadRequest(new { message = "There is no category with such id." });
            }
            var items = await _unitOfWork.ItemsRepository.GetItems(new Helpers.ItemParams() { CategoryId = id });
            if(items != null)
                return BadRequest(new { message = "Sorry, could not delete this category - some items using it." });
            _unitOfWork.CategoriesRepository.DeleteCategory(category);
            if (await _unitOfWork.Complete())
            {
                return Ok(new { message = $"The {category.Name} category has been deleted."});
            }
            return BadRequest(new { message = "Something went wrong. Could not delete this catogory" });
        }

        [HttpPut]
        public async Task<ActionResult<Category>> UpdateCategory(CategoryDto categoryDto)
        {
            var oldCategory = await _unitOfWork.CategoriesRepository.GetCategoryById(categoryDto.Id);
            if(oldCategory == null) 
            {
                return BadRequest(new { message = "There is no category with such id." });
            }
            _unitOfWork.CategoriesRepository.UpdateCategory(oldCategory, categoryDto);
            if(await _unitOfWork.Complete())
            {
                return Ok(oldCategory);
            }
            return BadRequest(new { message = "Something went wrong, could not update the category." });
        }
    }
}
