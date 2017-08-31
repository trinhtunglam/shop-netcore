using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Repositories
{
    public interface ISlideRepository : IEntityRepository<Slide>
    {
    }

    public partial class SlideRepository : EntityRepository<Slide>, ISlideRepository
    {
        public SlideRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
