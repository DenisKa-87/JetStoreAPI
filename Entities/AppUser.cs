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
            var user = new AppUser();
            user.Name = registerDto.Name;
            user.Surname = registerDto.Surname;
            user.Paternal = registerDto.Paternal;
            user.UserName = registerDto.Email;
            user.Email = registerDto.Email;
            user.DateOfBirth = registerDto.DateOfBirth;
            user.CreatedAt = DateOnly.FromDateTime(DateTime.Now);
            return user;
        }

    }
}
