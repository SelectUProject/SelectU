using SelectU.Contracts.Entities;
using SelectU.Contracts.Repositories;
using SelectU.Core.Repositories.Generic;
using SelectU.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectU.Core.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(SelectUContext context) : base(context)
        {
        }
    }
}
