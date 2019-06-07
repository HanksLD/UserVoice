using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UserVoice.Entity.IRepositories;
using System.Linq;

namespace UserVoice.Repository
{
    public class BaseRepository<TEntity, TKey> : IBaseRepository<TEntity, TKey> where TEntity : class, new()
    {
        private DbContext _context;

        protected BaseRepository(DbContext context)
        {
            this._context = context;
        }

        public bool Insert(TEntity entity)
        {
            if (null == entity)
                return false;
            Table.Add(entity);
            return true;
        }

        public Task<EntityEntry<TEntity>> InsertAsync(TEntity entity)
        {
            return Table.AddAsync(entity);
        }

        public TEntity Update(TEntity entity)
        {
            AttachIfNot(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        public void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        public void Delete(IEnumerable<TEntity> entities)
        {
            foreach (TEntity item in entities)
                Delete(item);
        }

        public TEntity First()
        {
            return Table.First();
        }

        public Task<TEntity> FirstAsync()
        {
            return Table.FirstAsync();
        }

        public TEntity First(Expression<Func<TEntity, bool>> where)
        {
            if (null == where)
                return null;
            return Table.First(where);
        }

        public Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> where)
        {
            if (null == where)
                return null;
            return Table.FirstAsync(where);
        }

        public DbContext DbContext => _context;

        public DbSet<TEntity> Table
        {
            get
            {
                return _context.Set<TEntity>();
            }
        }

        public IQueryable<TEntity> Query()
        {
            return Table.AsQueryable<TEntity>();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> where)
        {
            if (null != where)
                return Table.Where(where);
            else
                return Query();
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            if (!Table.Local.Contains(entity))
            {
                Table.Attach(entity);
            }
        }

        public int SaveChanged()
        {
            return _context.SaveChanges();
        }
    }
}
