using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


//Чтобы предоставить доступ к объектам Order, мы последуем тому же самому шаб­
//лону, который использовался для хранилища товаров (интерфейс IProductRepository)
//созаем интерйес IOrderRepository
namespace SportsStore.Models
{
    public interface IOrderRepository
    {
        IEnumerable<Order> Orders { get; }
        void SaveOrder(Order order);
    }
}
