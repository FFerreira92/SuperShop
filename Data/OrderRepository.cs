using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public class OrderRepository : GenericRepository<Order>,IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrderRepository(DataContext context,IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            
            if (user == null)
            {
                return;
            }

            var product = await _context.Products.FindAsync(model.ProductId);

            if (product == null)
            {
                return;
            }

            var orderDetailTemp = await _context.OrderDetailsTemps.Where(odt => odt.User == user && odt.Product == product).FirstOrDefaultAsync();

            if(orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailTemp
                {
                    Price = product.Price,
                    Product = product,
                    Quantity = model.Quantity,
                    User = user,
                };

                _context.OrderDetailsTemps.Add(orderDetailTemp);
            }
            else
            {
                orderDetailTemp.Quantity += model.Quantity;
                _context.OrderDetailsTemps.Update(orderDetailTemp);
            }

            await _context.SaveChangesAsync();

        }

        public async Task DeleteDetailTempAsync(int id)
        {

            var orderDetailTemp = await _context.OrderDetailsTemps.FindAsync(id);

            if(orderDetailTemp == null)
            {
                return;
            }


            _context.OrderDetailsTemps.Remove(orderDetailTemp);
            await _context.SaveChangesAsync();

        }

        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            return _context.OrderDetailsTemps.Include(p => p.Product).Where(o => o.User == user).OrderBy(o => o.Product.Name);
        }

        public async Task<IQueryable<Order>> GetOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            
            if(user == null)
            {
                return null;
            }

            if(await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .OrderByDescending(o => o.OrderDate);
            }

            return _context.Orders.Include(o => o.Items).ThenInclude(p => p.Product).Where(o => o.User == user).OrderByDescending(o => o.OrderDate);

        }

        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            var orderDetailTemp = await _context.OrderDetailsTemps.FindAsync(id);
            if(orderDetailTemp == null)
            {
                return;
            }

            orderDetailTemp.Quantity += quantity;
            if(orderDetailTemp.Quantity > 0)
            {
                _context.OrderDetailsTemps.Update(orderDetailTemp);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ConfirmOrderAsync(string username)
        {
            var user = await _userHelper.GetUserByEmailAsync(username);

            if(user == null)
            {
                return false;
            }

            var orderTemps = await _context.OrderDetailsTemps.Include(o => o.Product).Where(o => o.User == user).ToListAsync();

            if(orderTemps == null || orderTemps.Count == 0)
            {
                return false;
            }

            //faz conversão de encomenda temporaria para objeto do tipo detalhe de encomenda (lista)
            var details = orderTemps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Product = o.Product,
                Quantity = o.Quantity
            }).ToList();


            //faz conversão para encomenda
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                User = user,
                Items = details
            };

            await CreateAsync(order);

            //remove os items temporarios do utilizador
            _context.OrderDetailsTemps.RemoveRange(orderTemps);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeliverOrder(DeliveryViewModel model)
        {
            var order = await _context.Orders.FindAsync(model.Id);
            if(order == null)
            {
                return;
            }

            order.DeliveryDate = model.DeliveryDate;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }
    }
}
