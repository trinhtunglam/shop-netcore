using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Repositories
{
    public interface ICustomerRepository : IEntityRepository<Customer>
    {
    }

    public partial class CustomerRepository : EntityRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
