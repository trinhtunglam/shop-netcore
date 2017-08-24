using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Repositories
{
    public interface IProducerRepository : IEntityRepository<Producer>
    {
    }

    public partial class ProducerRepository : EntityRepository<Producer>, IProducerRepository
    {
        public ProducerRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
