using BUSINESS_OBJECTS;
using DATA_ACCESS.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICES
{
    public interface IInfomationService
    {
        IEnumerable<Infomation> GetAll();
        Infomation GetSingleById(int id);
        void Insert(Infomation entity);
        void Update(Infomation entity);
        void Delete(int id);
    }

    public class InfomationService : IInfomationService
    {
        private readonly IInfomationRepository _infomationRepository;

        public InfomationService(IInfomationRepository infomationRepository)
        {
            _infomationRepository = infomationRepository;
        }

        public void Delete(int id)
        {
            _infomationRepository.Delete(id);
        }

        public IEnumerable<Infomation> GetAll()
        {
            return _infomationRepository.GetAll();
        }

        public Infomation GetSingleById(int id)
        {
            return _infomationRepository.GetSingleById(id);
        }

        public void Insert(Infomation entity)
        {
            _infomationRepository.Insert(entity);
        }

        public void Update(Infomation entity)
        {
            _infomationRepository.Update(entity);
        }
    }
}
