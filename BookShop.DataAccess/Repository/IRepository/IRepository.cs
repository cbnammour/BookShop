using System.Linq.Expressions;

namespace BookShop.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // Eg: T - Category
        T GetFirsrOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        void Add(T entity);
        // Update is not consistent across all models, we don't implement it in the repository pattern
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
