using BUSINESS_OBJECTS;
using DATA_ACCESS.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICES
{
    public interface ICustomerService
    {
        IEnumerable<Customer> GetAll();
        Customer GetSingleById(int id);
        void Insert(Customer entity);
        Customer Add(Customer entity);
        void Update(Customer entity);
        void Delete(int id);
        bool CheckLogin(string email, string passWord);
        Customer GetSingleByEmail(string email);
    }

    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public Customer Add(Customer entity)
        {
            return _customerRepository.Add(entity);
        }

        public bool CheckLogin(string email, string passWord)
        {
            var result = _customerRepository.GetSingleByCondition(t => t.Email == email && t.Password == passWord);
            if (result != null)
                return true;
            return false;
        }

        public void Delete(int id)
        {
            _customerRepository.Delete(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }

        public Customer GetSingleByEmail(string email)
        {
            return _customerRepository.GetSingleByCondition(t => t.Email == email);
        }

        public Customer GetSingleById(int id)
        {
            return _customerRepository.GetSingleById(id);
        }

        public void Insert(Customer entity)
        {
            _customerRepository.Insert(entity);
        }

        public void Update(Customer entity)
        {
            _customerRepository.Update(entity);
        }
    }
}
