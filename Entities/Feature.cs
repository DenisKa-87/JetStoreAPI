
namespace JetStoreAPI.Entities
{
    public class Feature
    {
        public int Id { get; set; }
        public string?  Name { get; set; }
        public string? Value { get; set; }
        public Item Item { get; set; }



        public static Feature Create(string item1, string item2, Item item)
        {
            return new Feature()
            {
                Name = item1,
                Value = item2,
                Item = item
            };
        }
    }
}
