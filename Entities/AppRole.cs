using Microsoft.AspNetCore.Identity;

namespace JetStoreAPI.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
