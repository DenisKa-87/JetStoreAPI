using JetStoreAPI.DTO;

namespace JetStoreAPI.Entities
{
    public class MeasureUnit
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public bool IntOnly { get; set; }

        public static MeasureUnit Create(MeasureUnitDto measureUnitDto)
        {
            return new MeasureUnit
            {
                Name = measureUnitDto.Name,
                IntOnly = measureUnitDto.IntOnly,
            };
        }
    }
}
