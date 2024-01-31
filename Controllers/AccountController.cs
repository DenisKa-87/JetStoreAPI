using JetStoreAPI.Data;
using JetStoreAPI.DTO;
using JetStoreAPI.Entities;
using JetStoreAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JetStoreAPI.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AppUserDto>> LogIn(LoginDto loginDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == loginDto.Email.ToUpper());
            if (user == null)
            {
                return Unauthorized("The user with such email is not found.");
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if(!result.Succeeded)
                return BadRequest(new { message = "Something went wrong, could not log in" });
            var userDto = AppUserDto.Create(user);
            userDto.Token = await _tokenService.GetToken(user);
            return Ok(userDto);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUserDto>> RegisterUser(RegisterDto registerDto)
        {
            var user = AppUser.Create(registerDto);
            if(await UserExists(user))
                return BadRequest(new {message = "User with this email already exists."});

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, "Customer");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            var userDto = AppUserDto.Create(user);
            userDto.Token = await _tokenService.GetToken(user);
            return Ok(userDto);
        }

        [HttpPost("create-new-employee")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult> RegisterNewEmployee(RegisterDto registerDto)
        {
            var user = AppUser.Create(registerDto);
            if (await UserExists(user))
                return BadRequest(new { message = "User with this email already exists." });
            var result = await _userManager.CreateAsync(user, "Passw0rd");
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            var roleResult = await _userManager.AddToRoleAsync(user, "Employee");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            return Ok(new { message = $"The user '{string.Join(' ', user.Name, user.Paternal, user.Surname)}' has been registered." });
        }


        [HttpPut("update-account")]
        [Authorize]
        public async Task<ActionResult<AppUserDto>> UpdateAccount(RegisterDto registerDto)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return BadRequest();
            var newUser = AppUser.Create(registerDto);
            if (await UserExists(newUser))
                return BadRequest(new { message = $"User with {newUser.Email} already exists." });
            user.Name = newUser.Name;
            user.Surname = newUser.Surname;
            user.Email = newUser.Email;
            user.Paternal = newUser.Paternal;
            user.DateOfBirth = newUser.DateOfBirth;
            user.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            _unitOfWork.UsersRepository.UpdateUser(user);
            
            if (await _unitOfWork.Complete())
            {
                var userDto = AppUserDto.Create(user);
                userDto.Token = await _tokenService.GetToken(user);
                return Ok(userDto);
            } 
            return BadRequest(new { message = "Failed to update user"});


        }

        protected async Task<bool> UserExists(AppUser appUser)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedUserName == appUser.UserName.ToUpper());
            if(user == null)
                return false; 
            return true;
        }
    }
}
