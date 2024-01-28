using JetStoreAPI.Interfaces;

namespace JetStoreAPI.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly DataContext _context;

        public UsersRepository(DataContext context)
        {
            _context = context;
        }
    }
}
