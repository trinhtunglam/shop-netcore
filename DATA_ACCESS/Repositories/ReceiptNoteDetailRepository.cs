using BUSINESS_OBJECTS;
using DATA_ACCESS.Implements;
using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Repositories
{
    public interface IReceiptNoteDetailRepository : IEntityRepository<ReceiptNoteDetail>
    {
    }

    public partial class ReceiptNoteDetailRepository : EntityRepository<ReceiptNoteDetail>, IReceiptNoteDetailRepository
    {
        public ReceiptNoteDetailRepository(ShopOnlineDbContext entitiesContext) : base(entitiesContext)
        {
        }
    }
}
