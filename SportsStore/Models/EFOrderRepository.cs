using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{

    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;

        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<Order> Orders => context.Orders.Include(o => o.Lines).ThenInclude(l => l.Products);

        public void SaveOrder(Order order)
        {

            context.AttachRange(order.Lines.Select(l => l.Products));
            if (order.OrderID == 0)
            {
                context.SaveChanges();
            }
        }
    }
}
