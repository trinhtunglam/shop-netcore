using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace DATA_ACCESS.Interfaces
{
    public interface IEntityRepository<T> where T : class, new()
    {
        T GetSingleById(int id);
        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);
        IEnumerable<T> GetAll(string[] includes = null);
        IEnumerable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);
        void Insert(T entity);
        T Add(T entity);
        void Update(T entity);
        T UpdateResult(T entity);
        void Delete(T entity);
        void Delete(int id);
        bool CheckContains(Expression<Func<T, bool>> predicate);
        void InsertRange(List<T> entities, int batchSize = 100);
    }
}
