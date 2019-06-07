using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UserVoice.Entity.IRepositories
{
    public interface IBaseRepository<TEntity,TKey> where TEntity : class,new()
    {
        bool Insert(TEntity entity);

        Task<EntityEntry<TEntity>> InsertAsync(TEntity entity);

        TEntity Update(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        void Delete(TEntity entity);

        void Delete(IEnumerable<TEntity> entities);

        TEntity First();

        Task<TEntity> FirstAsync();

        TEntity First(Expression<Func<TEntity, bool>> where);

        Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> where);

        IQueryable<TEntity> Query();

        IQueryable<TEntity> Query(Expression<Func<TEntity,bool>> where);

        DbContext DbContext { get; }

        int SaveChanged();
    }
}
