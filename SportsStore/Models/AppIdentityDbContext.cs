using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// файл контекста базы данных, который будет дейс­твовать в качестве шлюза между базой данных и объектам и моделей Identity, 
// пре­доставляющими к ней доступ.
namespace SportsStore.Models
{
    #region AppidentityDbContext
    // Класс AppidentityDbContext является производным от класса Identi tyDb
    // Context, который предлагает связанные с Identity средства для Entity Framework
    // Core. В параметре типа используется Identi tyUser, представляющий собой встроен­
    // ный класс, который применяется для представления пользователей.В главе 28 будет
    // продемонстрировано использование специального класса, который можно расширять
    // с целью добавления дополнительной информации о пользователях приложения.
    #endregion
    public class AppIdentityDbContext : IdentityDbContext<IdentityUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }
    }

    // AppIdentityDbContext наследует IdentityDbContext в которм находится в которм находится ся реализция 
    // на уровне БД , IdentityDbContext - содержит все наборы DBSet коорые явл проводниками в нужные таблицы
    // которые нужны для работы Identity , тд полостью готовая механика для работы с БД на уровне безопасности 
    // через Identity.
}
