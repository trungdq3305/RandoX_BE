
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RandoX.Data.Bases
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task Add(TEntity entity);
        Task Add(IEnumerable<TEntity> entities);

        TEntity GetById(object id);

        IQueryable<TEntity> GetAll();

        Task Update(TEntity entity);

        Task Update(IEnumerable<TEntity> entities);

        void Remove(int id);

        void Remove(TEntity entity);

        void Remove(params TEntity[] entities);

        void Remove(IEnumerable<TEntity> entities);

        Task<List<TEntity>> Get(Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
            int? pageIndex = null, // Optional parameter for pagination (page number)
            int? pageSize = null, params Expression<Func<TEntity, object>>[] includes);

    }
}
