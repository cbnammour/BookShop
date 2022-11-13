using BookShop.Models;

namespace BookShop.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        // Update Cover Type
        void Update(Company obj);
    }
}
