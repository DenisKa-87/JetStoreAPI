namespace JetStoreAPI.Helpers
{
    public class ItemParams : Params
    {
        public string? Name { get; set; }
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
        public double? MinQuantity { get; set; }
        public double? MaxQuantity { get; set; }
        public int? CategoryId { get; set; } = 0;

    }
}
