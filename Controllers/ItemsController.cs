using JetStoreAPI.DTO;
using JetStoreAPI.Entities;
using JetStoreAPI.Extensions;
using JetStoreAPI.Helpers;
using JetStoreAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using SQLitePCL;

namespace JetStoreAPI.Controllers
{
    public class ItemsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public ItemsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems(
            [FromQuery] string? name,
            [FromQuery] string? minPrice,
            [FromQuery] string? maxPrice,
            [FromQuery] string? categoryId,
            [FromQuery] string? minQuantity,
            [FromQuery] string? maxQuantity,
            [FromQuery] string? order
            )
        {
            ItemParams query = CreateQuery(name, minPrice, maxPrice, categoryId ,
                minQuantity, maxQuantity, order);
            var items = await _unitOfWork.ItemsRepository.GetItems(query);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(int id)
        {
            var item = await _unitOfWork.ItemsRepository.GetItemById(id);
            if(item == null)
            {
                return BadRequest(new { message = "Sorry, there is no item with such id." }); ;
            }
            return Ok(item);
        }

        [HttpPost]
        [Authorize (Policy = "RequireEmployeeRole")]
        public async Task<ActionResult<ItemDto>> AddItem(ItemDto itemDto)
        {
            Category category = await _unitOfWork.CategoriesRepository.GetCategoryById(itemDto.CategoryId);
            if (category == null)
                return BadRequest(new { message = "Failed to add the item. No such category." });
            MeasureUnit measureUnit =
                await _unitOfWork.MeasureUnitsRepository.GetMeasureUnitById(itemDto.MeasureUnitId);
            if (measureUnit == null)
                return BadRequest(new { message = "Failed to add the item. No such unit of measure." });
            
            var item = await ItemExists(Item.CreateItem(itemDto, category, measureUnit));
            if(item != null) 
            {
                return BadRequest(new { message = "Sorry, this item already exists. Please change it if needed." });
            }
            
            item = Item.CreateItem(itemDto,category, measureUnit);
            _unitOfWork.ItemsRepository.AddItem(item);
            if(await _unitOfWork.Complete())
                return Ok(item);  
            return BadRequest(new { message = "Failed to add the item."});

        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireEmployeeRole")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var item = await _unitOfWork.ItemsRepository.GetItemById(id);
            if (item == null)
                return BadRequest(new { message = "Sorry, the item with such id is not found."});
            _unitOfWork.ItemsRepository.DeleteItem(item);
            if (_unitOfWork.HasChanges())
                if (await _unitOfWork.Complete())
                    return Ok(new { message = $"Item {item.Name} was deleted." });
            return BadRequest(new { message = "Something went wrong. Could not delete the item." });
        }

        [HttpPut]
        [Authorize(Policy = "RequireEmployeeRole")]
        public async Task<ActionResult<Item>> UpdateItem(ItemDto itemDto)

        {
            var item = await _unitOfWork.ItemsRepository.GetItemById(itemDto.Id);
            if (item == null)
                return BadRequest(new { message = "Sorry, there is no such item" });
            var category = await _unitOfWork.CategoriesRepository.GetCategoryById(itemDto.CategoryId);
            if (category == null)
                return BadRequest(new { mesage = "Sorry, there is no such category." });
            MeasureUnit measureUnit =
                await _unitOfWork.MeasureUnitsRepository.GetMeasureUnitById(itemDto.MeasureUnitId);
            if (measureUnit == null)
                return BadRequest(new { message = "Sorry, there is no such unit of measure." });
            var updatedItem = Item.CreateItem(itemDto, category, measureUnit);
            _unitOfWork.ItemsRepository.UpdateItem(item, updatedItem);
            if (await _unitOfWork.Complete())
                return Ok(item);
            return BadRequest(new { message = "Failed to update the item." });



        }
        private async Task<Item> ItemExists(Item item)
        {
            if(item.Id > 0)
            {
                return await _unitOfWork.ItemsRepository.GetItemById(item.Id);
            }
            var items = await _unitOfWork.ItemsRepository.GetItemsByName(item.Name);
            if(items.Count() == 0)
            {
                return null;
            }
            foreach(var foundItem in items)
            {
                if (Item.ItemsAreEqual(foundItem, item))
                    return foundItem;
            }
            return null;
        }


        private ItemParams CreateQuery(string? name = null, string? minPrice = null, string? maxPrice = null,
            string? categoryId = null, string? minQuantity = null, string? maxQuantity = null, string? order = null)
        {
            var query = new ItemParams();
            query.Name = name;
            query.Order = order;
            query.CategoryId = categoryId.ParseInt();
            query.MinPrice = minPrice.ParseDouble();
            query.MaxPrice = maxPrice.ParseDouble();
            query.MinQuantity = minQuantity.ParseDouble();
            query.MaxQuantity = maxQuantity.ParseDouble();

            return query;

        }
    }
}
