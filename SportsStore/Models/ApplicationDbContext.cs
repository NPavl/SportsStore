using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#region что такое контекст 
//Чтобы подключиться к базе данных через Entity Framework, нам нужен контекст данных.
//Контекст данных представляет собой класс, производный от класса DbContext.
//Контекст данных содержит одно или несколько свойств типа DbSet<T>, где T представляет 
//тип объекта, хранящегося в базе данных.
//----
//Класс контекста базы данных является шлюзом между приложением и EF Core, 
//обеспечивая доступ к данным приложения с применением объектов моделей.
//Создадим класс конткста базы данных ApplicationDbContext для нашего приложения
#endregion

namespace SportsStore.Models
{
    #region DbContext
    // Базовый класс DbContext представляет сеанс с базой данных и может использоваться для
    // запрашиваем и сохраняем экземпляры ваших сущностей. DbContext является комбинацией
    // Единица работы и шаблоны репозитория.

    // Базовый класс DbContext предоставляет доступ к лежащей в основе функцио ­
    // налыюсти Entity Fraшework Core, а свойство Products обеспечивает доступ к объек­
    // там Products в базе данных. Для наполнения базы данных добавьте в папку Models
    // файл 1-шасса по имени SeedData.cs
    #endregion
    public class ApplicationDbContext : DbContext // класс DbContext определяет контекст данных, используемый для взаимодействия с базой данных.
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        {                
        }
        //-----------------------------------------------------------
        //protected string Connection;
        //public ApplicationDbContext(string connectionString) : base()
        //{
        //    Connection = connectionString;
        //}

        //public ApplicationDbContext()
        //{
        //    Connection = "Server=(localdb)\\MSSQLLocalDB;Database=SportsStore;Trusted_Connection=True;MultipleActiveResultSets=true";
        //}
        //-----------------------------------------------------------
        #region Products
        // спомощью свойства Products мы будем поллучать доступ к данным существующих моделей.
        // свойство Products обеспечивает доступ к объек­там Product в базе данных.
        #endregion
        public DbSet<Products> Products { get; set; } // класс DbSet представляет набор сущностей, хранящихся в базе данных
        public DbSet<Order> Orders { get; set; } // добавив данное свойство является достаточным основанием 
        //для инфраструктуры EntityFramework Core создать миграцию базы данных, которая по зволит объектам Order
        //сохраняться в базе данных. 
    }
}


