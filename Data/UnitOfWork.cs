using JetStoreAPI.Interfaces;

namespace JetStoreAPI.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context) 
        {
            _context = context;
        }
        
        public IUsersRepository UsersRepository => new UsersRepository(_context);

        public ICategoriesRepository CategoriesRepository => new CategoriesRepository(_context);

        public IMeasureUnitsRepository MeasureUnitsRepository => new MeasureUnitRepository(_context);

        public IItemsRepository ItemsRepository => new ItemsRepository(_context);

        public async Task<bool> Complete()
        {
            return (await _context.SaveChangesAsync() > 0);
        }

        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }
    }
}
