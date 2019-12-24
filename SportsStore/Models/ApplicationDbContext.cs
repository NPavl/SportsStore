using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options)
        {  }  
       
        public DbSet<Products> Products { get; set; } 
        public DbSet<Order> Orders { get; set; }
       
    }
}


