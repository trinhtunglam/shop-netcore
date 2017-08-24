using BUSINESS_OBJECTS;
using DATA_ACCESS.Repositories;
using SERVICES.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICES
{
    public interface ISupplierService
    {
        Supplier GetSingleById(int id);
        IEnumerable<Supplier> GetAll();
        void Insert(Supplier entity);
        void Update(Supplier entity);
        void Delete(int id);
    }
    public class SupplierService : ISupplierService
    {
        private const string PRODUCT_ALL_KEY = "Supplier.All";

        private readonly ISupplierRepository _supplierRepository;
        private readonly ICacheManager _cacheManager;

        public SupplierService(ISupplierRepository supplierRepository, ICacheManager cacheManager)
        {
            _supplierRepository = supplierRepository;
            _cacheManager = cacheManager;
        }
        public void Delete(int id)
        {
            _supplierRepository.Delete(id);
            _cacheManager.Set(PRODUCT_ALL_KEY, _supplierRepository.GetAll(), 1200000);
        }

        public IEnumerable<Supplier> GetAll()
        {
            if (_cacheManager.Get<IEnumerable<Supplier>>(PRODUCT_ALL_KEY) == null)
            {
                _cacheManager.Set(PRODUCT_ALL_KEY, _supplierRepository.GetAll(), 1200000);
                return _cacheManager.Get<IEnumerable<Supplier>>(PRODUCT_ALL_KEY);
            }
            return _cacheManager.Get<IEnumerable<Supplier>>(PRODUCT_ALL_KEY);
        }

        public Supplier GetSingleById(int id)
        {
            return _supplierRepository.GetSingleById(id);
        }

        public void Insert(Supplier entity)
        {
            _supplierRepository.Insert(entity);
            _cacheManager.Set(PRODUCT_ALL_KEY, _supplierRepository.GetAll(), 1200000);
        }

        public void Update(Supplier entity)
        {
            _supplierRepository.Update(entity);
            _cacheManager.Set(PRODUCT_ALL_KEY, _supplierRepository.GetAll(), 1200000);
        }
    }
}
