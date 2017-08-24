using BUSINESS_OBJECTS;
using DATA_ACCESS.Repositories;
using SERVICES.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICES
{
    public interface IReceiptNoteDetailService
    {
        ReceiptNoteDetail GetSingleById(int id);
        IEnumerable<ReceiptNoteDetail> GetAll();
        IEnumerable<ReceiptNoteDetail> GetAllByReceiptId(int id);
        void Insert(ReceiptNoteDetail entity);
        void Update(ReceiptNoteDetail entity);
        void Delete(int id);
        void InsertRanger(List<ReceiptNoteDetail> entities);
    }
    public class ReceiptNoteDetailService : IReceiptNoteDetailService
    {
        private const string PRODUCT_ALL_KEY = "Product.All";

        private readonly IReceiptNoteDetailRepository _receiptNoteDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICacheManager _cacheManager;

        public ReceiptNoteDetailService(IReceiptNoteDetailRepository receiptNoteDetailRepository,
            ICacheManager cacheManager,
            IProductRepository productRepository)
        {
            _receiptNoteDetailRepository = receiptNoteDetailRepository;
            _productRepository = productRepository;
            _cacheManager = cacheManager;
        }
        public void Delete(int id)
        {
            _receiptNoteDetailRepository.Delete(id);
        }

        public IEnumerable<ReceiptNoteDetail> GetAll()
        {
            return _receiptNoteDetailRepository.GetAll();
        }

        public IEnumerable<ReceiptNoteDetail> GetAllByReceiptId(int id)
        {
            return _receiptNoteDetailRepository.GetMulti(t => t.ReceiptNodeId == id,new string[] {"ReceiptNote", "Product" });
        }

        public ReceiptNoteDetail GetSingleById(int id)
        {
            return _receiptNoteDetailRepository.GetSingleById(id);
        }

        public void Insert(ReceiptNoteDetail entity)
        {
            _receiptNoteDetailRepository.Insert(entity);
        }

        public void InsertRanger(List<ReceiptNoteDetail> entities)
        {
            var allData = _productRepository.GetAll();
            _receiptNoteDetailRepository.InsertRange(entities);
            _cacheManager.Set(PRODUCT_ALL_KEY, allData, 1200000);

        }

        public void Update(ReceiptNoteDetail entity)
        {
            
            _receiptNoteDetailRepository.Update(entity);
            _cacheManager.Set(PRODUCT_ALL_KEY, _productRepository.GetAll(), 1200000);
        }
    }
}
