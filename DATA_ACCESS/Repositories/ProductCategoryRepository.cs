using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Repositories
{
    public interface IProductCategoryRepository : IEntityRepository<ProductCategory>
    {
    }

    public partial class ProductCategoryRepository: EntityRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
