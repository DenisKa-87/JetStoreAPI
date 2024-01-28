using Microsoft.AspNetCore.Identity;

namespace JetStoreAPI.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public DateOnly CreatedAt { get; set; }

        public AppUser(string Name, string Surname, DateOnly dateOfBirth)
        {
            this.Name = Name;
            this.Surname = Surname;
            DateOfBirth = dateOfBirth;
            CreatedAt = DateOnly.FromDateTime(DateTime.Now);
        }

    }
}
