using JetStoreAPI.Entities;

namespace JetStoreAPI.DTO
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int MeasureUnitId { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Tuple<string, string>>? Features { get; set; }
    }
}
