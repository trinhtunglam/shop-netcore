using DATA_ACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DATA_ACCESS.Implements
{
    public class EntityRepository<T> : IEntityRepository<T> where T : class, new()
    {
        protected readonly DbContext _entitiesContext;

        public EntityRepository(DbContext entitiesContext)
        {
            if (entitiesContext == null)
            {
                throw new ArgumentNullException("entitiesContext");
            }

            _entitiesContext = entitiesContext;
        }

        private DbSet<T> _entities;

        private DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _entitiesContext.Set<T>();
                }
                return _entities as DbSet<T>;
            }
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        public virtual void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual IEnumerable<T> GetAll(string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = _entitiesContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.AsQueryable();
            }

            return _entitiesContext.Set<T>().AsQueryable();//.AsQueryable();
        }

        public virtual IEnumerable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = _entitiesContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.Where<T>(predicate).AsQueryable<T>();
            }

            return _entitiesContext.Set<T>().Where<T>(predicate).AsQueryable<T>();
        }

        public virtual T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = _entitiesContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return query.FirstOrDefault(expression);
            }
            return _entitiesContext.Set<T>().FirstOrDefault(expression);
        }

        public virtual T GetSingleById(int id)
        {
            return this.Entities.Find(id);
        }

        public virtual void Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Add(entity);

                this._entitiesContext.SaveChanges();
            }
            catch (Exception dbEx)
            {
                throw dbEx;
            }
        }

        public virtual void Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                var dbEntityEntry = _entitiesContext.Entry<T>(entity);
                dbEntityEntry.State = EntityState.Modified;
                this._entitiesContext.SaveChanges();
            }
            catch (Exception dbEx)
            {
                throw dbEx;
            }
        }

        public virtual void Delete(int id)
        {
            try
            {
                var entity = this.Entities.Find(id);
                this.Entities.Remove(entity);

                this._entitiesContext.SaveChanges();
            }
            catch (Exception dbEx)
            {
                throw dbEx;
            }
        }

        public bool CheckContains(Expression<Func<T, bool>> predicate)
        {
            return _entitiesContext.Set<T>().Count<T>(predicate) > 0;
        }

        public void InsertRange(List<T> entities, int batchSize = 100)
        {
            try
            {
                if (entities == null)
                    throw new ArgumentNullException("entities");

                if (entities.Any())
                {
                    if (batchSize <= 0)
                    {
                        // insert all in one step
                        entities.ForEach(x => this.Entities.Add(x));
                        _entitiesContext.SaveChanges();
                    }
                    else
                    {
                        int i = 1;
                        bool saved = false;
                        foreach (var entity in entities)
                        {
                            this.Entities.Add(entity);
                            saved = false;
                            if (i % batchSize == 0)
                            {
                                _entitiesContext.SaveChanges();
                                i = 0;
                                saved = true;
                            }
                            i++;
                        }

                        if (!saved)
                        {
                            _entitiesContext.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual T Add(T entity)
        {
            this.Entities.Add(entity);

            this._entitiesContext.SaveChanges();
            return entity;
        }

        public T UpdateResult(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                var dbEntityEntry = _entitiesContext.Entry<T>(entity);
                dbEntityEntry.State = EntityState.Modified;
                this._entitiesContext.SaveChanges();
                return entity;
            }
            catch (Exception dbEx)
            {
                throw dbEx;
            }
        }
    }
}
