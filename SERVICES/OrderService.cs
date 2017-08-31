using BUSINESS_OBJECTS;
using DATA_ACCESS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SERVICES
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAll();
        IEnumerable<Order> GetAll(string searchString);
        IEnumerable<Order> GetBySelectId(int id);
        IEnumerable<Order> GetOrderByCustomerEmail(string email);
        Order GetById(int id);
        IEnumerable<OrderDetail> GetByOrderId(int id);
        bool Create(Order order, List<OrderDetail> orderDetail);
        Order UpdateResult(Order order);
    }
    public class OrderService : IOrderService
    {
        IOrderRepository _orderRepository;
        IOrderDetailRepository _orderDetailRepository;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
        }
        public bool Create(Order order, List<OrderDetail> orderDetail)
        {
            try
            {
                order.CreatedDate = DateTime.Now;
                _orderRepository.Add(order);

                foreach (var itemDetail in orderDetail)
                {
                    itemDetail.OrderId = order.Id;
                    _orderDetailRepository.Add(itemDetail);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderRepository.GetAll().OrderBy(c => c.CreatedDate);
        }

        public IEnumerable<Order> GetAll(string searchString)
        {
            return _orderRepository.GetMulti(t => t.CustomerMobile.Contains(searchString));
        }

        public Order GetById(int id)
        {
            return _orderRepository.GetSingleById(id);
        }

        public IEnumerable<OrderDetail> GetByOrderId(int id)
        {
            return _orderDetailRepository.GetMulti(t => t.OrderId == id,new string[] { "Product"});
        }

        public IEnumerable<Order> GetBySelectId(int id)
        {
            if (id == 1)
                return _orderRepository.GetMulti(t => t.Status == true);
            else if(id == 2)
                return _orderRepository.GetAll();
            return _orderRepository.GetMulti(t => t.Status == false);
        }

        public IEnumerable<Order> GetOrderByCustomerEmail(string email)
        {
            return _orderRepository.GetMulti(t => t.CustomerEmail == email);
        }

        public Order UpdateResult(Order order)
        {
            return _orderRepository.UpdateResult(order);
        }
    }
}
