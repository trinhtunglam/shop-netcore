using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Repositories
{
    public interface IOrderDetailRepository : IEntityRepository<OrderDetail>
    {
    }

    public partial class OrderDetailRepository : EntityRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
