using JetStoreAPI.DTO;
using Microsoft.AspNetCore.Identity;

namespace JetStoreAPI.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public string Paternal {  get; set; }
        public DateOnly? DateOfBirth { get; set; }

        public DateOnly CreatedAt { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }

        public static AppUser Create(RegisterDto registerDto)
        {
            var user = new AppUser
            {
                Name = registerDto.Name,
                Surname = registerDto.Surname,
                Paternal = registerDto.Paternal,
                UserName = registerDto.Email,
                Email = registerDto.Email,
                DateOfBirth = registerDto.DateOfBirth,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now)
            };
            return user;
        }

    }
}
