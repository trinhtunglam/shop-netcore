using BUSINESS_OBJECTS;
using DATA_ACCESS.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICES
{
    public interface ISlideService
    {
        IEnumerable<Slide> GetAll();
        IEnumerable<Slide> GetSlide();
        Slide GetById(int id);
        void Insert(Slide entity);
        void Update(Slide entity);
        void Delete(int id);
    }
    public class SlideService : ISlideService
    {
        private readonly ISlideRepository _slideRepository;
        private readonly IBannerRepository _bannerRepository;

        public SlideService(ISlideRepository slideRepository, IBannerRepository bannerRepository)
        {
            _slideRepository = slideRepository;
            _bannerRepository = bannerRepository;
        }
        public void Delete(int id)
        {
            _slideRepository.Delete(id);
        }

        public IEnumerable<Slide> GetAll()
        {
            return _slideRepository.GetAll();
        }


        public Slide GetById(int id)
        {
            return _slideRepository.GetSingleById(id);
        }

        public IEnumerable<Slide> GetSlide()
        {
            return _slideRepository.GetMulti(t => t.Status == true);
        }

        public void Insert(Slide entity)
        {
            _slideRepository.Insert(entity);
        }

        public void Update(Slide entity)
        {
            _slideRepository.Update(entity);
        }
    }
}
