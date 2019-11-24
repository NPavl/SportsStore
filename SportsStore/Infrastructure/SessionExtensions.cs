using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Infrastructure
{
    #region описание класса
    //Средство состояния сеанса в ASP.NET Core хранит только значения int, string
    //и byte[]. Поскольку мы хотим сохранять объект Cart, необходимо определить рас­
    //ширяющие методы для интерфейса ISession, которые предоставят доступ к дан­
    //ным состояния сеанса с целью сериализации объектов Cart в формат JSON и их об­
    //ратного преобразования.Добавьте в папку Infrastructure файл класса по имени
    //SessionExtensions.cs с определениями расширяющих методов
    #endregion
    public static class SessionExtensions
    {
        // расширяющие методы состояния сеанса (сериализуют и десериализуют (сохраняют) обькт Cart в формате Json)
        // для добавления обькта Cart используется метод SetJson, для извлечения GetJson

        #region описание методов SetJson GetJson
        //При сериализации объектов в формат JSON(JavaScript Object Notation - систе­
        //ма обозначений для объектов JavaScript) эти методы полагаются на пакет Json.NET
        //(глава 20). Пакет Json.NET не потребуется добавлять в файл package. j son, т.к.он
        //уже используется "за кулисами" инфраструктурой MVC для поддержки средства заго­
        //ловков JSON, которое описано в главе 21. (И нформация о работе с пакетом Json.NET
        //напрямую доступна по адресу www . newtonsoft.сот/ j son.)
        //Расширяющие методы делают сохранение и извлечение объектов Cart очень лег­
        //ким.Для добавления объекта Cart к состоянию сеанса в контроллере применяется
        //следующий вызов: HttpContext.Session.SetJson(Cart,cart);

        //Свойство HttpContext определено в базовом классе Controller, от которого
        //обычно унаследованы контроллеры, и возвращает объект HttpContext.Этот объект
        //предоставляет данные контекста о запросе, который был получен, и ответе, находя­
        //щемся в процессе подготовки.Свойство HttpContext. Session возвращает объект,
        //реализующий интерфейс ISession.Данный интерфейс является именно тем типом,
        //где мы определили метод SetJson (), принимающий аргументы, в которых указы-
        //ваются ключ и объект, подлежащий добавлению в состояние сеанса.Расширяющий
        //метод сериализирует объект и добавляет его в состояние сеанса, используя функцио­
        //нальность, которая лежит в основе интерфейса ISession. 
        //Чтобы извлечь объект Cart, применяется другой расширяющий метод, которому
        //указывается тот же самый ключ:  Cart cart = HttpContext.Session.GetJson<Cart>(Cart);
        //Параметр типа позволяет задать тип объекта, который ожидается извлечь; этот
        //используется в процессе десериализации.
        #endregion

        public static void SetJson(this ISession session, string key, object value) 
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static T GetJson<T>(this ISession session, string key)   
        {
            var sessionData = session.GetString(key);
            return sessionData == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
        }
    }
}
