using BUSINESS_OBJECTS;
using DATA_ACCESS.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICES
{
    public interface IUserManagerService
    {
        IEnumerable<ApplicationUser> GetAll();
    }
    public class UserManagerService : IUserManagerService
    {
        private readonly IUserManagerRepository _userManager;

        public UserManagerService(IUserManagerRepository userManager)
        {
            _userManager = userManager;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _userManager.GetAll();
        }
    }
}
