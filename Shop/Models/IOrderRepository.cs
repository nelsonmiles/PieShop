using System.Collections.Generic;

namespace Shop.Models
{
    public interface IOrderRepository
    {
        //IEnumerable<Order> Orders { get; }

        void CreateOrder(Order order);
        void CreatePieGiftOrder(PieGiftOrder pieGiftOrder);
    }
}
