using JetStoreAPI.DTO;
using JetStoreAPI.Entities;
using JetStoreAPI.Helpers;

namespace JetStoreAPI.Interfaces
{
    public interface IItemsRepository
    {
        void AddItem(Item item);
        void UpdateItem(Item oldItem, Item newItem);
        void DeleteItem(Item item);
        Task<IEnumerable<Item>> GetItemsByName(string name);
        Task<Item> GetItemById(int id);
        Task<IEnumerable<Item>> GetItems(ItemParams? itemParams);

    }
}
