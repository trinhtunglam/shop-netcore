using BUSINESS_OBJECTS;
using DATA_ACCESS.Repositories;
using SERVICES.Caching;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICES
{
    public interface IProductCategoryService
    {
        IEnumerable<ProductCategory> GetAll();
        ProductCategory GetSingleById(int id);
        void Insert(ProductCategory entity);
        void Update(ProductCategory entity);
        void Delete(int id);
    }

    public class ProductCategoryService: IProductCategoryService
    {
        private const string PRODUCT_ALL_KEY = "ProductCategory.All";

        private readonly IProductCategoryRepository _productCategory;
        private readonly ICacheManager _cacheManager;

        public ProductCategoryService(IProductCategoryRepository productCategory, ICacheManager cacheManager)
        {
            _productCategory = productCategory;
            _cacheManager = cacheManager;
        }

        public void Delete(int id)
        {
            _productCategory.Delete(id);
            _cacheManager.Set(PRODUCT_ALL_KEY, _productCategory.GetAll(), 1200000);
        }

        public IEnumerable<ProductCategory> GetAll()
        {
            //if (_cacheManager.Get<IEnumerable<ProductCategory>>(PRODUCT_ALL_KEY) == null)
            //{
            //    _cacheManager.Set(PRODUCT_ALL_KEY, _productCategory.GetAll(), 1200000);
            //    return _cacheManager.Get<IEnumerable<ProductCategory>>(PRODUCT_ALL_KEY);
            //}
            //return _cacheManager.Get<IEnumerable<ProductCategory>>(PRODUCT_ALL_KEY);
            return _productCategory.GetAll();
        }

        public ProductCategory GetSingleById(int id)
        {
            return _productCategory.GetSingleById(id);
        }

        public void Insert(ProductCategory entity)
        {
            _productCategory.Insert(entity);
            _cacheManager.Set(PRODUCT_ALL_KEY, _productCategory.GetAll(), 1200000);
        }

        public void Update(ProductCategory entity)
        {
            _productCategory.Update(entity);
            _cacheManager.Set(PRODUCT_ALL_KEY, _productCategory.GetAll(), 1200000);
        }
    }
}
