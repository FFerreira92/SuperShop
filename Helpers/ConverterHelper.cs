using System.IO;
using SuperShop.Data.Entities;
using SuperShop.Models;

namespace SuperShop.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Product toProduct(ProductViewModel model,string path ,bool isNew)
        {
            return new Product
            {
                Id = isNew ? 0 : model.Id,
                ImageUrl = path,
                IsAvailable = model.IsAvailable,
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                User = model.User,
            };
        }

        public ProductViewModel toProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                IsAvailable = product.IsAvailable,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                ImageUrl = product.ImageUrl,
                Stock = product.Stock,
                Name = product.Name,
                Price = product.Price,
                User = product.User
            };
        }
    }
}
