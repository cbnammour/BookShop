using BookShop.Models;

namespace BookShop.DataAccess.Repository.IRepository
{
    public interface IOrderHeaderRepository : IRepository<OrderHeader>
    {
        // Update category
        void Update(OrderHeader obj);
        void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
    }
}
