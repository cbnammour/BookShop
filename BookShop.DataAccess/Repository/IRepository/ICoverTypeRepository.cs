using BookShop.Models;

namespace BookShop.DataAccess.Repository.IRepository
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        // Update Cover Type
        void Update(CoverType obj);
    }
}
