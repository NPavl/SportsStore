using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;
using Newtonsoft.Json;

namespace SportsStore.Models
{
    // Создание класса корзины, осведомленного о хранилище 

    #region описание класса SessionCart
    //Первым шагом по приведению в порядок способа использования класса Cart
    //будет создание подкласса SessionCart, который осведомлен о том, как сохранять самого себя
    //с применением состояния сеанса.

    //Класс SessionCart является производным от класса Cart и переопределяет мето­
    //ды AddItem(), RemoveLine() и Clear() ,так что они вызывают базовые реализации
    //и затем сохраняют обновленное состояние в сеансе, используя расширяющие мето­
    //ды интерфейса ISession, которые были определ ены в главе 9. Статический метод
    //GetCart() - это фабрика для создания объектов SessionCart и их пр едоставления
    //с помощью объекта реализации ISession, так что они могут себя сохранять.
    //Получение объекта реализации ISession несколько затруднено.Мы должны по­
    //лучить экземпляр службы IHttpContextAccessor, который предоставит доступ к
    //объекту HttpContext, а тот, в свою очередь, к объекту реализации ISession. Такой
    //окольный подход требуется из -за того, что сеанс не предоставляется как обычная
    //служба. 
    #endregion
    public class SessionCart : Cart
    {
        // метод GetCart фабрика для создания объектов SessionCart
        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart") ?? new SessionCart();
            cart.Session = session;
            return cart;
        }
        [JsonIgnore]
        public ISession Session { get; set; }

        public override void AddItem(Products product, int quantity)
        {
            base.AddItem(product, quantity);
            Session.SetJson("Cart", this);
        }

        public override void RemoveLine(Products product)
        {
            base.RemoveLine(product);
            Session.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }

    }
}
