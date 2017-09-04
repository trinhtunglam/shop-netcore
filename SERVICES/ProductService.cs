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
        IEnumerable<Product> GetByCategoryHome();
        IEnumerable<Product> GetByProducer(int producerId);
        IEnumerable<Product> GetByCategoryByProducer(int categoryId,int producerId);
        IEnumerable<Product> GetListProductByName(string keyword);
        IEnumerable<Product> GetProductNew();
        IEnumerable<Product> GetProductBest();
        IEnumerable<Product> GetProductRelated(int id);
        IEnumerable<ProductCategory> GroupBy();
        IEnumerable<Producer> GroupByProducer();

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
        private readonly IProductCategoryRepository _categoryRepository;
        private readonly IProducerRepository _producerRepository;
        private readonly ICacheManager _cacheManager;

        public ProductService(IProductRepository productRepository, ICacheManager cacheManager,
            IProductCategoryRepository categoryRepository,
            IProducerRepository producerRepository)
        {
            _productRepository = productRepository;
            _producerRepository = producerRepository;
            _categoryRepository = categoryRepository;
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
            //if (_cacheManager.Get<IEnumerable<Product>>(PRODUCT_ALL_KEY) == null)
            //{
            //    _cacheManager.Set(PRODUCT_ALL_KEY, _productRepository.GetAll(new string[] { "ProductCategory", "Producer" }), 1200000);
            //    return _cacheManager.Get<IEnumerable<Product>>(PRODUCT_ALL_KEY);
            //}
            //return _cacheManager.Get<IEnumerable<Product>>(PRODUCT_ALL_KEY);
            _cacheManager.Set(PRODUCT_ALL_KEY, _productRepository.GetAll(new string[] { "ProductCategory", "Producer" }), 1200000);
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

        public IEnumerable<Product> GetByProducer(int producerId)
        {
            return _productRepository.GetMulti(t => t.ProducerId == producerId);
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

        public IEnumerable<Product> GetProductBest()
        {
            return _productRepository.GetAll().OrderBy(t => t.CreateDate);
        }

        public IEnumerable<Product> GetProductRelated(int id)
        {
            var product = _productRepository.GetSingleById(id);
            return _productRepository.GetMulti(t => t.Id != id && t.CategoryId == product.CategoryId && t.ProducerId==product.ProducerId).OrderByDescending(t=>t.CreateDate);
        }

        public IEnumerable<Product> GetProductNew()
        {
            return _productRepository.GetAll().OrderByDescending(t => t.CreateDate);
        }

        public Product GetSingleById(int id)
        {
            return _productRepository.GetSingleByCondition(t=>t.Id==id,new string[] {"Supplier"});
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

        public IEnumerable<ProductCategory> GroupBy()
        {
            var model = from c in _productRepository.Table
                        group c by c.ProductCategory into g
                        select new ProductCategory {
                            Id=g.Key.Id,
                            Name=g.Key.Name
                        };
            return model;
        }

        public IEnumerable<Producer> GroupByProducer()
        {
            var model = from c in _productRepository.Table
                        group c by c.Producer into g
                        select new Producer
                        {
                            Id = g.Key.Id,
                            Name = g.Key.Name
                        };
            return model;

            //var model = from c in _productRepository.Table
            //            group c by c.ProductCategory into g
            //            select c;

            //List<Producer> lstProducer = new List<Producer>();
            //var model1 = from c in model
            //             group c by c.Producer into g
            //             select new Producer
            //             {
            //                 Id = g.Key.Id,
            //                 Name = g.Key.Name
            //             };
            //return lstProducer;

        }

        public IEnumerable<Product> GetByCategoryByProducer(int categoryId, int producerId)
        {
            return _productRepository.GetMulti(t => t.ProducerId == producerId && t.CategoryId==categoryId);
        }

        public IEnumerable<Product> GetByCategoryHome()
        {
            var category = _categoryRepository.GetAll();
            IEnumerable<Product> lstProduct = new List<Product>();
            foreach (var item in category)
            {
                var model = GetByCategory(item.Id);
                lstProduct = model;
            }
            return lstProduct;
        }
    }
}
