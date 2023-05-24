using SelectU.Contracts.Repositories;

namespace SelectU.Contracts
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        Task CommitAsync();
    }
}
