using JetStoreAPI.DTO;
using JetStoreAPI.Entities;
using JetStoreAPI.Helpers;
using JetStoreAPI.Interfaces;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Reflection;
using System.Xml.Linq;

namespace JetStoreAPI.Data
{
    public class ItemsRepository : IItemsRepository
    {
        private readonly DataContext _context;

        public ItemsRepository(DataContext context)
        {
            _context = context;
        }
        public void AddItem(Item item)
        {
            _context.Add(item);
        }

        public void DeleteItem(Item item)
        {
            _context.Remove(item);
        }

        public async Task<Item> GetItemById(int id)
        {
            return await _context.Items.Include(x => x.Category).Include(x => x.Unit).Include(x => x.Features)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Item>> GetItemsByName(string name)
        {
            return await _context.Items.Include(x => x.Category).Include(x => x.Unit).Where(x => x.Name.ToLower() == name.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetItems(ItemParams? itemParams = null)
        {
            if (itemParams == null)
                return await _context.Items.ToListAsync();
            var items = _context.Items.Include(x => x.Category).Include(x => x.Unit).Include(x => x.Features)
                .AsQueryable();
            if (itemParams.Name != null)
                items = items.Where(x => x.Name.ToLower() == itemParams.Name.ToLower());
            if (itemParams.CategoryId != null || itemParams.CategoryId > 0)
                items = items.Where(x => x.Category.Id == itemParams.CategoryId);
            if(itemParams.MinPrice != null)
                items = items.Where(x => x.Price >= itemParams.MinPrice);
            if (itemParams.MaxPrice != null)
                items = items.Where(x => x.Price <= itemParams.MaxPrice);
            if (itemParams.MinQuantity != null)
                items = items.Where(x => x.Quantity >= itemParams.MinQuantity);
            if (itemParams.MaxQuantity != null)
                items = items.Where(x => x.Quantity <= itemParams.MaxQuantity);
            return await items.ToListAsync();


        }

        public void UpdateItem(Item oldItem, Item newItem)
        {

            ChangeItemProps(oldItem, newItem);
        }

        private static void ChangeItemProps(Item item, Item newItem)
        {
            
            
            item.Name = newItem.Name;
            item.Price = newItem.Price;
            item.Unit = newItem.Unit;
            item.Quantity = newItem.Quantity;
            item.Description = newItem.Description;
            item.ImageUrl = newItem.ImageUrl;
            item.Category = newItem.Category;            
            item.Features = newItem.Features;

        }
    }
}
