using JetStoreAPI.Data;
using JetStoreAPI.DTO;
using JetStoreAPI.Helpers;
using JetStoreAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JetStoreAPI.Controllers
{
    [Authorize(Policy = "RequireEmployeeRole")]
    public class UsersController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUserDto>>> GetAppUsersAsync(
            [FromQuery] string? name,
            [FromQuery] string? paternal,
            [FromQuery] string? surname,
            [FromQuery] string? email,
            [FromQuery] string? birthday
            )
        {
            var userParams = UserParams.Create(name, paternal, surname, email, birthday);
            var users = await _unitOfWork.UsersRepository.GetUsers(userParams);
            var result = new List<AppUserDto>();
            foreach(var user in users)
            {
                result.Add(new AppUserDto
                {
                    Name = user.Name,
                    Surname = user.Surname,
                    Paternal = user.Paternal,
                    Email = user.Email,
                    DateOfBirth = user.DateOfBirth
                });
            }
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUserDto>> GetUserById(int id)
        {
            var user = await _unitOfWork.UsersRepository.GetUserById(id);
            if(user == null) 
                return BadRequest(new { message = "There is no user with such Id."});
            return Ok(new AppUserDto
            {
                Name = user.Name,
                Surname = user.Surname,
                Paternal = user.Paternal,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth

            });
        }
    }
}
