using BookShop.Models;

namespace BookShop.DataAccess.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        // Update category
        void Update(OrderDetail obj);
    }
}
