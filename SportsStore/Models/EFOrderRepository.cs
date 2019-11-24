using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    //класс Для реализации интерфейса хранилища (IOrderRepository)  

    //Класс EFOrderRepository реализует интерфейс IOrderRepository с примене­
    //нием Entity Framework Core. позволяя извлекать набор сохраненных объектов Order
    //и создавать либо изменять заказы.

    public class EFOrderRepository : IOrderRepository
    {
        private ApplicationDbContext context;

        public EFOrderRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        #region методы      Thenlnclude
        // с помощью ме­тодов Include() и Thenlnclude() указано, что когда класс Order читается из базы
        // данных, то также должна загружаться коллекция, ассоциированная со свойством Lines, 
        // наряду с объектами Product, связанными с элементами коллекции
        // Это гарантирует получение всех объектов данных, которые нужны, не выполняя запросы и
        // не собирая данные напрямую.
        #endregion
        public IEnumerable<Order> Orders => context.Orders.Include(o => o.Lines).ThenInclude(l => l.Products);

        public void SaveOrder(Order order)
        {
            #region AttachRange
            // AttachRange Начинает отслеживать заданные объекты в Microsoft.EntityFrameworkCore.EntityState.Unchanged
            // состояние, при котором Microsoft.EntityFrameworkCore.DbContext.SaveChanges не будет выполнять никаких операций.

            // Дополнительный шаг требуется также и при сохранении объекта Order в базе данных. 
            // Когда данные корзины пользователя десериализируются из состояния сеанса, пакет JSON
            // создает новые объекты, не известные инфраструктуре Eпtity Framework Core, которая затем
            // пытается записать все объекты в базу данных. В случае объектов Products это означает, 
            // что инфраструктура EF Core попытается записать объекты, которые уже были сохранены,
            // что приведет к ошибке. Во избежание проблемы мы уведомляем Eпtity Framework Core о
            // том, что объекты существуют и не должны сохраняться в базе данных до тех пор, пока они
            // не будут модифицированы. 

            // В результате инфраструктура EF Core не будет пытаться записывать десериализированные
            // объекты Products, которые ассоциированы с объектом Order. 
            #endregion
            context.AttachRange(order.Lines.Select(l => l.Products));
            if (order.OrderID == 0)
            {
                context.SaveChanges();
            }
        }
    }
}
