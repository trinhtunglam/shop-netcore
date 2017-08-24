using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Repositories
{
    public interface IProductRepository : IEntityRepository<Product>
    {
    }

    public partial class ProductRepository : EntityRepository<Product>, IProductRepository
    {
        public ProductRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
