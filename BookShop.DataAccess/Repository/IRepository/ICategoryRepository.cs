using BookShop.Models;

namespace BookShop.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        // Update category
        void Update(Category category);
    }
}
