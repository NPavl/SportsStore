using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    // класс который реалезует интерфейс IProductRepository и получает данные 
    //с применением EF core
    public class EFProductRepository : IProductRepository
    {
        private ApplicationDbContext context;
        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }
        public IEnumerable<Products> Products => context.Products; // это свойство вытягивает данные из базы

        //на текущем контексте с базой данных вызывается свойтво Products(DbSet) из класса ApplicationDbContext
        //и присваетсва ткущему свойству этого класса EFProductRepository данные из базы в свойство Products


        #region описание SaveProduct() , SaveChanges()
        // данный метод позволит сохранять изменения в БД 

        // Реализация метода SaveChanges() добавляет товар в хранилище, если значение
        // ProductID равно О; в противном случае применяются изменения к существующей
        // записи в базе данных.
        // Мы не хотим здесь вдаваться в детали инфраструктуры Entity Framework Core, пос­
        // кольку, как упоминалось ранее, это отдельная крупная тема, к тому же она не является
        // частью ASP.NET Core МVС. Тем не менее, в методе SaveProduct () есть кое-что, что
        // оказьmает влияние на проектное решение, положенное в основу приложения МVС.

        // Нам известно, что обновление должно выполняться, когда получен параметр
        // Product, который имеет ненулевое значение ProductID.Это делается путем извле­
        // чения из хранилища объекта Product с тем же самым значением ProductID и об­
        // новления всех его свойств, чтобы они соответствовали значениям свойств объекта,
        // переданного в качестве параметра. 
        // Причина таких действий в том, что инфраструктура Entity Framework Core отсле­
        // живает объекты, которые она создает из базы данных. Объект, переданный методу
        // SaveChanges () , создается системой привязки моделей MVC , т.е.инфраструктура
        // Entity Framework Core ничего не знает о новом объекте Product, и она не будет при­
        // менять обновление к базе данных, иогда объеит Product модифицирован. Существует
        // множество способов решения уиазанной пробл емы, но мы принимаем самый простой
        // из них.предполагающий поиск соответствующего объекта, о котором известно инф­
        // раструктуре Entity Framework Core, и его явное обновление. 
        // Добавление нового метода в интерфейс IProductRepository нарушает работу
        // класса имитированного хранилища FakeProductReposi tory, который был создан
        // в главе 8. Имитированное хранилище использовалось для быстрого старта процес­
        // са разработки и демонстрации возможности применения служб для гладкой заме­
        // ны реализаций интерфейса, не изменяя компоненты, которые на них опираются. 
        // Имитированное хранилище больше не понадобится.В листинге 11.11 видно, что ин­
        // терфейс IProductReposi tory удален из объявления класса, поэтому продолжать мо­
        // дификацию класса по мере добавления функций хранилища не придется. 
        #endregion
        public void SaveProduct(Products product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Products dbEntry = context.Products
                    .FirstOrDefault(p => p.ProductID == product.ProductID);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                }
            }
            context.SaveChanges();
        }

        public Products DeleteProducts(int productID)
        {
            Products dbEntry = context.Products
                .FirstOrDefault(p => p.ProductID == productID);
            if (dbEntry != null)
            {
                context.Products.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
