using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Repositories
{
    public interface IOrderRepository : IEntityRepository<Order>
    {
    }

    public partial class OrderRepository : EntityRepository<Order>, IOrderRepository
    {
        public OrderRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
