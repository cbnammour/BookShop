using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        // Eg: T - Category
        T GetFirsrOrDefault(Expression<Func<T, bool>> filter);
        IEnumerable<T> GetAll();
        void Add(T entity);
        // Update is not consistent across all models, we don't implement it in the repository pattern
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
