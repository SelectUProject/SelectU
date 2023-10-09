using SelectU.Contracts.Repositories;

namespace SelectU.Contracts
{
    public interface IUnitOfWork
    {
        IScholarshipRepository Scholarships { get; }

        IScholarshipApplicationRepository ScholarshipApplications { get; }
        IUserRepository Users { get; }
        IReviewRepository Review { get; }
        Task CommitAsync();
    }
}
