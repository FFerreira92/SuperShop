using System.Linq;
using SuperShop.Data.Entities;

namespace SuperShop.Data
{
    public interface IProductsRepository : IGenericRepository<Product>
    {
        public IQueryable GetAllWithUsers();
    }
}
