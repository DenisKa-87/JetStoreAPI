using JetStoreAPI.Entities;

namespace JetStoreAPI.Interfaces
{
    public interface ITokenService
    {
        Task<string> GetToken(AppUser user);
    }
}
