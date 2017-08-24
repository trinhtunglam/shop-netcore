using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Repositories
{

    public interface ISupplierRepository : IEntityRepository<Supplier>
    {
    }

    public partial class SupplierRepository : EntityRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
