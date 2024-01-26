using System.Linq.Expressions;

namespace NoheasApparel.DataAccess.Repository.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {


        IEnumerable<TEntity> GetAll(string? includeProperties = null);

        TEntity Find(Expression<Func<TEntity, bool>> predicate, string? includeProperties = null);

        void Add(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        void Delete(TEntity entity);

        void DeleteRange(IEnumerable<TEntity> entities);
    }
}
