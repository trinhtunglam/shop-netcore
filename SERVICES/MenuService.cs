using BUSINESS_OBJECTS;
using DATA_ACCESS.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICES
{

    public interface IMenuService
    {
        IEnumerable<Menu> GetAll();
        Menu GetSingleById(int id);
        void Insert(Menu entity);
        void Update(Menu entity);
        void Delete(int id);
    }

    public class MenuService : IMenuService
    {
        private readonly IMenuRepository _menuRepository;

        public MenuService(IMenuRepository menuRepository)
        {
            _menuRepository = menuRepository;
        }

        public void Delete(int id)
        {
            _menuRepository.Delete(id);
        }

        public IEnumerable<Menu> GetAll()
        {
            return _menuRepository.GetAll();
        }

        public Menu GetSingleById(int id)
        {
            return _menuRepository.GetSingleById(id);
        }

        public void Insert(Menu entity)
        {
            _menuRepository.Insert(entity);
        }

        public void Update(Menu entity)
        {
            _menuRepository.Update(entity);
        }
    }
}
