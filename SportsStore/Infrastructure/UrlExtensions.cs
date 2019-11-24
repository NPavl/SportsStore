using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Infrastructure
{
    //создали даный класс чтобы модифицировать частичное представление Views / Shared/ 
    //ProductSummary.cshtml. добавив кнопки к спискам товаров.

    public static class UrlExtensions
    {
        #region PathAndQuery
        // Расширяющий метод PathAndQuery() оперирует над классом HttpRequest ( его мы и расширили методом (PathAndQuery)), ис­
        // пользуемый инфраструктурой ASP.NET для описания НТТР-запроса. Расширяющий
        // метод генерирует URL, по которому браузер будет возвращаться после обновления кор­
        // зины. принимая во внимание строку запроса. если она есть. В листинге 9.15 к файлу
        // импортирования представлений добавляется пространство имен, которое содержит
        // расширяющий метод. так что его можно применять в частичном представлении.
        #endregion
        #region расширяяющий метод понятие
        //    Методы расширения(extension methods) позволяют добавлять новые методы 
        //    в уже существующие типы без создания нового производного класса.
        //    Эта функциональность бывает особенно полезна, когда нам хочется добавить 
        //    в некоторый тип новый метод, но сам тип(класс или структуру) мы изменить 
        //    не можем(private), поскольку у нас нет доступа к исходному коду. Либо если мы не 
        //    можем использовать стандартный механизм наследования, например, если 
        //    классы определенны с модификатором sealed.
        //Для того, чтобы создать метод расширения, вначале надо создать статический класс, который и будет содержать этот метод.
        // https://metanit.com/sharp/tutorial/3.18.php  быстрый пример
        #endregion
        public static string PathAndQuery(this HttpRequest request) =>
            request.QueryString.HasValue ? $"{request.Path}{request.QueryString}" : request.Path.ToString();

    }



}
