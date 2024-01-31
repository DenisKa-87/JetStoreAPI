using JetStoreAPI.DTO;
using JetStoreAPI.Entities;
using JetStoreAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JetStoreAPI.Controllers
{
    [Authorize(Policy = "RequireEmployeeRole")]
    public class MeasureUnitController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public MeasureUnitController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MeasureUnit>>> GetMeasureUnits()
        {
            var units = await _unitOfWork.MeasureUnitsRepository.GetMeasureUnits();
            return Ok(units);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MeasureUnit>> GetMeasureUnitById(int id)
        {
            var unit = await _unitOfWork.MeasureUnitsRepository.GetMeasureUnitById(id);
            if (unit == null)
            {
                return BadRequest(new { message = "Sorry, there is no such unit." });
            }
            return Ok(unit);
        }

        [HttpPost]
        public async Task<ActionResult<MeasureUnit>> AddMeasureUnit(MeasureUnitDto measureUnitDto)
        {
            var measureUnit = MeasureUnit.Create(measureUnitDto);
            if (await MeasureUnitExists(measureUnit))
            {
                return BadRequest(new { message = "Failed to add measure unit. This unit already exists." });
            }
            _unitOfWork.MeasureUnitsRepository.AddMeasureUnit(measureUnit);
            if (await _unitOfWork.Complete())
                return Ok(measureUnit);
            return BadRequest(new { message = "Failed to add measure unit." });
        }

        [HttpPut]
        public async Task<ActionResult<MeasureUnit>> UpdateMeasureUnit(MeasureUnitDto measureUnitDto)
        {
            var measureUnit = await _unitOfWork.MeasureUnitsRepository.GetMeasureUnitById(measureUnitDto.Id);
            if (measureUnit == null)
                return BadRequest(new { message = "Measure with such Id does not exist." });
            var newUnit = MeasureUnit.Create(measureUnitDto);
            if (await MeasureUnitExists(newUnit))
                return BadRequest(new { message = "Such measure unit already exists." });
            measureUnit.Name = measureUnitDto.Name == "" || measureUnitDto.Name == null ? measureUnit.Name : measureUnitDto.Name;
            measureUnit.IntOnly = measureUnitDto.IntOnly;
            if (await _unitOfWork.Complete())
                return Ok(measureUnit);
            return BadRequest(new { message = "Something went wrong. Could not update this measure unit" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUnit(int id)
        {
            var unit = await _unitOfWork.MeasureUnitsRepository.GetMeasureUnitById(id);
            if (unit == null) return BadRequest(new { message = "Sorry, there is no such unit of measure." });
            var items = await _unitOfWork.ItemsRepository.GetItems(null);
            if(items.Any(x => x.Unit == unit))
                return BadRequest(new { message = "Sorry, could not delete this unit - some items are using it." });
            _unitOfWork.MeasureUnitsRepository.DeleteMeasureUnit(unit);

            if (await _unitOfWork.Complete())
                return Ok(new { message = $"Unit {unit.Name} has been deleted." });
            return BadRequest(new { message = "Something went wrong. Could not delete this measure unit" });
        }

        private async Task<bool> MeasureUnitExists(MeasureUnit measureUnit)
        {
            var units = await _unitOfWork.MeasureUnitsRepository.GetMeasureUnitByName(measureUnit.Name);
            return units.Where(x => x.IntOnly == measureUnit.IntOnly).Count() > 0;
        }


    }
}
