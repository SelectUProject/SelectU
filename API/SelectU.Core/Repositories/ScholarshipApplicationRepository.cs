using SelectU.Contracts.Entities;
using SelectU.Contracts.Repositories;
using SelectU.Core.Repositories.Generic;
using SelectU.Migrations;

namespace SelectU.Core.Repositories
{
    public class ScholarshipApplicationRepository : Repository<ScholarshipApplication>, IScholarshipApplicationRepository
    {
        public ScholarshipApplicationRepository(SelectUContext context) : base(context)
        {
        }
    }
}
