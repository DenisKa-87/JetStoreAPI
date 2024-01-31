namespace JetStoreAPI.Interfaces
{
    public interface IUnitOfWork
    {
        IItemsRepository ItemsRepository { get; }
        IUsersRepository UsersRepository { get; }
        ICategoriesRepository CategoriesRepository { get; }
        IMeasureUnitsRepository MeasureUnitsRepository { get; }

        bool HasChanges();
        Task<bool> Complete();
    }
}
