using JetStoreAPI.Entities;
using JetStoreAPI.Helpers;

namespace JetStoreAPI.Interfaces
{
    public interface IUsersRepository
    {

        void UpdateUser(AppUser newData);
        Task<AppUser> GetUserById(int id);
        Task<IEnumerable<AppUser>> GetUsers(UserParams userParams);
    }
}
