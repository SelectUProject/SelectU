using SelectU.Contracts.Repositories;

namespace SelectU.Contracts
{
    public interface IUnitOfWork
    {
        IScholarshipRepository Scholarships { get; }
        IUserRepository Users { get; }
        Task CommitAsync();
    }
}
