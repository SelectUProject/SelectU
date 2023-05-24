using SelectU.Contracts.Entities;
using SelectU.Contracts.Repositories;
using SelectU.Core.Repositories.Generic;
using SelectU.Migrations;

namespace SelectU.Core.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(SelectUContext context) : base(context)
        {
        }
    }
}
