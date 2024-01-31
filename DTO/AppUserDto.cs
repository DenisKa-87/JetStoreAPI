using JetStoreAPI.Entities;

namespace JetStoreAPI.DTO
{
    public class AppUserDto
    {
        public string Name { get; set;}
        public string Surname { get; set;}
        public string Paternal {  get; set;}
        public string Token { get; set;}
        public string Email { get; set; }
        public DateOnly? DateOfBirth { get; set; }

        public static AppUserDto Create(AppUser user) {
        
            return new AppUserDto
            {
                Name = user.Name,
                Surname = user.Surname,
                Paternal = user.Paternal,
                DateOfBirth = user.DateOfBirth,
            };
        }

    }

    
}
