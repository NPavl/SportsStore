using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    // класс SeedData наполняет БД  
    public class SeedData
    {
        #region EnsurePopulated
        //Статический метод EnsurePopulated() получает аргумент типа IApplicationBuilder, 
        //который является классом, используемым в методе Configure() нласса
        //Startup при регистрации классов промежуточного программного обеспечения для
        //обработки Н1ТР-запросов; именно здесь будет обеспечиваться наличие содержимого
        //в базе данных.
        //Метод The EnsurePopulated() получает объект ApplicationDbContext пос­
        //редством интерфейса IApplicationBuilder и применяет его для проверки, при­
        //сутствуют ли в базе данных какие-нибудь объекты Product. Если объектов нет, то
        //база данных наполняется с использованием коллекции объектов Product и метода
        //AddRange() ,после чего сохраняется с помощью метода SaveChanges().

        //большая часть работы, тре­
        //буемой для настройки базы данных, завершена. Следующий шаг заключается в со­
        //здании класса, который реализует интерфейс IProductRepository и получает дан­
        //ные с применением инфраструктуры Entity Framework Core. Добавьте в папку Mod els
        //файл класса по имени EFProductRepository.cs
        #endregion
        public static void EnsurePopulated(ApplicationDbContext context)
        {
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Products
                    {
                        Name = "Kayak",
                        Description = "A boat for one person",
                        Category = "Watersports",
                        Price = 275
                    },
                new Products
                {
                    Name = "Lifejacet",
                    Description = "Protective and fashionable",
                    Category = "Watersports",
                    Price = 48.95m
                },
                new Products
                {
                    Name = "Soccer Ball",
                    Description = "FIFA-approved size and weight",
                    Category = "Soccer",
                    Price = 19.50m
                },

                new Products
                {
                    Name = "Corner Flags",
                    Description = "Give your playing field a professional touch",
                    Category = "Soccer",
                    Price = 34.95m
                },
                new Products
                {
                    Name = "Stadium",
                    Description = "Flat-packed 35,000-seat stadium",
                    Category = "Soccer",
                    Price = 79500
                },
                new Products
                {
                    Name = "Unsteady Chair",
                    Description = "Secretly give your opponent a disadvantage",
                    Category = "Chess",
                    Price = 29.95m
                },
                new Products
                {
                    Name = "Thinking Сар",
                    Description = "Improve brain efficiency by 75%",
                    Category = "Chess",
                    Price = 16
                },
                new Products
                {
                    Name = "Human Chess Board",
                    Description = "A fun game for the family",
                    Category = "Chess",
                    Price = 75
                },
                new Products
                {
                    Name = "Bling-Bling King",
                    Description = "Gold-plated, diamond-studded King",
                    Category = "Chess",
                    Price = 1200
                }
               );
            }
            context.SaveChanges();
        }
    }
}
