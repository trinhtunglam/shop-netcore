using BUSINESS_OBJECTS;
using DATA_ACCESS.Repositories;
using SERVICES.Caching;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICES
{
    public interface IProductService
    {
        Product GetSingleById(int id);
        IEnumerable<Product> GetAll();
        IEnumerable<Product> GetAll(string searchString);
        IEnumerable<Product> GetListProductPaging(int page,int pageSize, out int totalRow);
        IEnumerable<Product> GetByCategory(int categoryId);
        IEnumerable<Product> GetListProductByName(string keyword);
        
        //Product GetSingleByWhere(int id);
        void Insert(Product entity);
        void Update(Product entity);
        void Delete(int id);
        bool CheckUser(string Code);
        bool SubTractionProduct(int productId, int quantity);
    }
    public class ProductService : IProductService
    {
        private const string PRODUCT_ALL_KEY = "Product.All";
        private const string COUNTRIES_PATTERN_KEY = "Product.Insert";

        private readonly IProductRepository _productRepository;
        private readonly ICacheManager _cacheManager;

        public ProductService(IProductRepository productRepository, ICacheManager cacheManager)
        {
            _productRepository = productRepository;
            _cacheManager = cacheManager;
        }

        public bool CheckUser(string Code)
        {
            return _productRepository.CheckContains(t => t.ProductCode == Code);
        }

        public void Delete(int id)
        {
            _productRepository.Delete(id);
            _cacheManager.Set(PRODUCT_ALL_KEY, _productRepository.GetAll(), 1200000);
        }

        public IEnumerable<Product> GetAll()
        {
            if (_cacheManager.Get<IEnumerable<Product>>(PRODUCT_ALL_KEY) == null)
            {
                _cacheManager.Set(PRODUCT_ALL_KEY, _productRepository.GetAll(), 1200000);
                return _cacheManager.Get<IEnumerable<Product>>(PRODUCT_ALL_KEY);
            }
            return _cacheManager.Get<IEnumerable<Product>>(PRODUCT_ALL_KEY);
        }

        public IEnumerable<Product> GetAll(string searchString)
        {
            return _productRepository.GetMulti(t=>t.ProductCode.Contains(searchString) || t.Name.Contains(searchString));
        }

        public IEnumerable<Product> GetByCategory(int categoryId)
        {
            return _productRepository.GetMulti(t => t.CategoryId == categoryId);
        }

        public IEnumerable<Product> GetListProductByName(string keyword)
        {
            return _productRepository.GetMulti(t => t.Name.Contains(keyword));
        }

        public IEnumerable<Product> GetListProductPaging(int page, int pageSize, out int totalRow)
        {
            var query = _productRepository.GetAll();
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize);
        }

        public Product GetSingleById(int id)
        {
            return _productRepository.GetSingleById(id);
        }

        public void Insert(Product entity)
        {
            _productRepository.Insert(entity);
            _cacheManager.Set(PRODUCT_ALL_KEY, _productRepository.GetAll(), 1200000);
        }

        public bool SubTractionProduct(int productId, int quantity)
        {
            var product = _productRepository.GetSingleById(productId);
            if (product.Quantity < quantity)
                return false;
            product.Quantity -= quantity;
            return true;
        }

        public void Update(Product entity)
        {
            _productRepository.Update(entity);
            _cacheManager.Set(PRODUCT_ALL_KEY, _productRepository.GetAll(), 1200000);
        }
    }
}
