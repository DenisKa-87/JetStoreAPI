using JetStoreAPI.Entities;
using JetStoreAPI.Helpers;
using JetStoreAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JetStoreAPI.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _context;

        public UsersRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<AppUser>> GetUsers(UserParams userParams = null)
        {
            return await _context.Users.ToListAsync();
        }

        public void UpdateUser(AppUser newData)
        {
            _context.Entry(newData).State = EntityState.Modified;   
        }
    }
}
