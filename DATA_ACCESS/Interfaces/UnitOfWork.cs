using System;
using System.Collections.Generic;
using System.Text;

namespace DATA_ACCESS.Interfaces
{
    public class UnitOfWork:IUnitOfWork,IDisposable
    {
        private readonly ShopOnlineDbContext _dbContext;

        public UnitOfWork(ShopOnlineDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        //public Repository<T> Repository<T>() where T : class
        //{
        //    if (repositories == null)
        //    {
        //        repositories = new Dictionary<string, object>();
        //    }

        //    var type = typeof(T).Name;

        //    if (!repositories.ContainsKey(type))
        //    {
        //        var repositoryType = typeof(Repository<>);
        //        var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
        //        repositories.Add(type, repositoryInstance);
        //    }
        //    return (Repository<t>)repositories[type];
        //}

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
