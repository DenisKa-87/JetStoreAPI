using JetStoreAPI.DTO;
using JetStoreAPI.Entities;
using JetStoreAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JetStoreAPI.Data
{
    public class MeasureUnitRepository : IMeasureUnitsRepository
    {
        private readonly DataContext _context;

        public MeasureUnitRepository(DataContext context)
        {
            _context = context;
        }

        public void AddMeasureUnit(MeasureUnit measureUnit)
        {
            _context.MeasureUnits.Add(measureUnit);
        }

        public void DeleteMeasureUnit(MeasureUnit measureUnit)
        {
            _context.MeasureUnits.Remove(measureUnit);
        }

        public async Task<MeasureUnit> GetMeasureUnitById(int measureUnitId)
        {
            return await _context.MeasureUnits.FindAsync(measureUnitId);
        }

        public async Task<IEnumerable<MeasureUnit>> GetMeasureUnitByName(string measureUnitName)
        {
            return await _context.MeasureUnits.Where(x => x.Name.ToLower() == measureUnitName.ToLower()).ToListAsync();
        }

        public async Task<IEnumerable<MeasureUnit>> GetMeasureUnits()
        {
            return await _context.MeasureUnits.ToListAsync();
        }

        public void UpdateMeasureUnit(MeasureUnit measureUnit, MeasureUnitDto measureUnitDto)
        {
            measureUnit.Name = measureUnitDto.Name;
            measureUnit.IntOnly = measureUnitDto.IntOnly;
        }
    }
}
