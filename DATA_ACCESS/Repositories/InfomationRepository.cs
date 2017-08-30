using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Repositories
{

    public interface IInfomationRepository : IEntityRepository<Infomation>
    {
    }

    public partial class InfomationRepository : EntityRepository<Infomation>, IInfomationRepository
    {
        public InfomationRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
