using BUSINESS_OBJECTS;
using DATA_ACCESS.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICES
{
    public interface IProducerService
    {
        Producer GetSingleById(int id);
        IEnumerable<Producer> GetAll();
        void Insert(Producer entity);
        void Update(Producer entity);
        void Delete(int id);
        
    }
    public class ProducerService : IProducerService
    {
        private const string PRODUCT_ALL_KEY = "Producer.All";

        private readonly IProducerRepository _producerRepository;

        public ProducerService(IProducerRepository producerRepository)
        {
            _producerRepository = producerRepository;
        }
        public void Delete(int id)
        {
            _producerRepository.Delete(id);
        }

        public IEnumerable<Producer> GetAll()
        {
            return _producerRepository.GetAll();
        }

        public Producer GetSingleById(int id)
        {
            return _producerRepository.GetSingleById(id);
        }

        public void Insert(Producer entity)
        {
            _producerRepository.Insert(entity);
        }

        public void Update(Producer entity)
        {
            _producerRepository.Update(entity);
        }
    }
}
