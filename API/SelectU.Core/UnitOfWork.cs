using Microsoft.EntityFrameworkCore;
using SelectU.Contracts;
using SelectU.Contracts.Repositories;
using SelectU.Core.Repositories;
using SelectU.Migrations;

namespace SelectU.Core
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SelectUContext _context;

        public UnitOfWork(SelectUContext context)
        {
            _context = context;
        }

        public IScholarshipApplicationRepository ScholarshipApplications => new ScholarshipApplicationRepository(_context);
        public IScholarshipRepository Scholarships => new ScholarshipRepository(_context);
        public IUserRepository Users => new UserRepository(_context);
        public IUserRatingRepository UserRating => new UserRatingRepository(_context);
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
