using JetStoreAPI.DTO;
using JetStoreAPI.Entities;

namespace JetStoreAPI.Interfaces
{
    public interface IMeasureUnitsRepository
    {
        Task<MeasureUnit> GetMeasureUnitById(int measureUnitId);
        Task<IEnumerable<MeasureUnit>> GetMeasureUnits();
        Task<IEnumerable<MeasureUnit>> GetMeasureUnitByName(string measureUnitName);
        void AddMeasureUnit(MeasureUnit measureUnit);
        void UpdateMeasureUnit(MeasureUnit measureUnit, MeasureUnitDto measureUnitDto);
        void DeleteMeasureUnit(MeasureUnit measureUnit);

    }
}
