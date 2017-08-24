using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Repositories
{
    public interface IReceiptNoteRepository : IEntityRepository<ReceiptNote>
    {
    }

    public partial class ReceiptNoteRepository : EntityRepository<ReceiptNote>, IReceiptNoteRepository
    {
        public ReceiptNoteRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
