using BookShop.Models;
using BookShop.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace BookShop.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }

    }
}