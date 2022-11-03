using BookShop.Models;

namespace BookShop.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        // Update Cover Type
        void Update(Product obj);
    }
}
