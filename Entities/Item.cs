using JetStoreAPI.DTO;
using System.Runtime.CompilerServices;

namespace JetStoreAPI.Entities
{
    
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double Price { get; set; }
        public double Quantity { get; set; }
        public Category Category { get; set; }
        public ICollection<Feature> Features { get; set; }
        public MeasureUnit Unit { get; set; }



        public static Item CreateItem(ItemDto itemDto, Category category, MeasureUnit measureUnit)
        {
            var item = new Item();
            item.Name = itemDto.Name;
            item.Price = itemDto.Price;
            item.Unit = measureUnit;
            item.Quantity = itemDto.Quantity;
            item.Description = itemDto.Description;
            item.ImageUrl = itemDto.ImageUrl;
            item.Category = category;
            item.Features = new List<Feature>();
            if (itemDto.Features != null && itemDto.Features.Count != 0)
            {
                foreach (var feature in itemDto.Features)
                {
                    item.Features.Add( Feature.Create(feature.Item1, feature.Item2, item));
                }
            }
            return item;
        }
        public static  bool ItemsAreEqual(Item item, Item item2)
        {
            bool result = false;
            result = item.Name.ToLower() == item2.Name.ToLower();
            result = item.Unit.Id == item2.Unit.Id 
                && item.Category.Id == item2.Category.Id;
            return result;
        }
    }

    
}
