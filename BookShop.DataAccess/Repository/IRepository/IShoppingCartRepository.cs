using BookShop.Models;

namespace BookShop.DataAccess.Repository.IRepository
{
    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        int incrementCount(ShoppingCart shoppingCart, int count);
        int decrementCart(ShoppingCart shoppingCart, int count);
    }
}
