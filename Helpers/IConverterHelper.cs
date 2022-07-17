using System;
using SuperShop.Data.Entities;
using SuperShop.Models;

namespace SuperShop.Helpers
{
    public interface IConverterHelper
    {
        Product toProduct(ProductViewModel model, Guid imageId ,bool isNew);

        ProductViewModel toProductViewModel(Product product);
    }
}
