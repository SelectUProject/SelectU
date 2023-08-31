using SelectU.Contracts.Entities;
using SelectU.Contracts.Repositories;
using SelectU.Core.Repositories.Generic;
using SelectU.Migrations;

namespace SelectU.Core.Repositories
{
    public class ScholarshipRepository : Repository<Scholarship>, IScholarshipRepository
    {
        public ScholarshipRepository(SelectUContext context) : base(context)
        {
        }
    }
}
