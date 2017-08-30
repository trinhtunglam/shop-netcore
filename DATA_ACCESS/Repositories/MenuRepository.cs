using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Repositories
{

    public interface IMenuRepository : IEntityRepository<Menu>
    {
    }

    public partial class MenuRepository : EntityRepository<Menu>, IMenuRepository
    {
        public MenuRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
