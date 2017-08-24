using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DATA_ACCESS.Repositories
{
    public interface IUserManagerRepository : IEntityRepository<ApplicationUser>
    {
    }

    public partial class UserManagerRepository : EntityRepository<ApplicationUser>, IUserManagerRepository
    {
        public UserManagerRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
