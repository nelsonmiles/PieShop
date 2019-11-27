using Shop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly ShoppingCart _shoppingCart;


        public OrderRepository(AppDbContext appDbContext, ShoppingCart shoppingCart)
        {
            _appDbContext = appDbContext;
            _shoppingCart = shoppingCart;
        }

        //View user order details
        //public IEnumerable<Order> Orders
        //    {
        //    get
        //        {
        //        return _appDbContext.Orders.Include(c => c.Category);
        //        }
        //    }


        public void CreateOrder(Order order)
        {
            
            order.OrderPlaced = DateTime.Now;
           



            _appDbContext.Orders.Add(order);

            var shoppingCartItems = _shoppingCart.ShoppingCartItems;

            foreach (var shoppingCartItem in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Amount = shoppingCartItem.Amount,
                    PieId = shoppingCartItem.Pie.PieId,
                    OrderId = order.OrderId,
                    Price = shoppingCartItem.Pie.Price
                };

                _appDbContext.OrderDetails.Add(orderDetail);

               
            }

          

            _appDbContext.SaveChanges();
        }

       


        public void CreatePieGiftOrder(PieGiftOrder pieGiftOrder)
        {
            _appDbContext.PieGiftOrders.Add(pieGiftOrder);
            _appDbContext.SaveChanges();
        }
    }
}
