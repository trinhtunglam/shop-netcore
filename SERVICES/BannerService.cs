using BUSINESS_OBJECTS;
using DATA_ACCESS.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICES
{
    public interface IBannerService
    {
        IEnumerable<Banner> GetAll();
        IEnumerable<Banner> GetBannerByCategory(int categoryId);
        Banner GetById(int id);
        void Insert(Banner entity);
        void Update(Banner entity);
        void Delete(int id);
    }
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public void Delete(int id)
        {
            _bannerRepository.Delete(id);
        }

        public IEnumerable<Banner> GetAll()
        {
            return _bannerRepository.GetAll(new string[] { "ProductCategory"});
        }

        public IEnumerable<Banner> GetBannerByCategory(int categoryId)
        {
            return _bannerRepository.GetMulti(t => t.Status == true && t.CategoryId == categoryId);
        }

        public Banner GetById(int id)
        {
            return _bannerRepository.GetSingleById(id);
        }

        public void Insert(Banner entity)
        {
            _bannerRepository.Insert(entity);
        }

        public void Update(Banner entity)
        {
            _bannerRepository.Update(entity);
        }
    }
}
