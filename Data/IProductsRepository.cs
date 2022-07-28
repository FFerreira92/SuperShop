using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuperShop.Data.Entities;

namespace SuperShop.Data
{
    public interface IProductsRepository : IGenericRepository<Product>
    {
        public IQueryable GetAllWithUsers();

        public IEnumerable<SelectListItem> GetComboProducts();
    }
}
