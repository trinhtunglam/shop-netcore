﻿using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Repositories
{
    public interface IBannerRepository : IEntityRepository<Banner>
    {
    }

    public partial class BannerRepository : EntityRepository<Banner>, IBannerRepository
    {
        public BannerRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
