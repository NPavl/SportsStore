using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository repository;
        private Cart cart;

        //модифицируем конструктор так , чтобы он получал службы, требующиеся ему для обработки заказа.
        public OrderController(IOrderRepository repoService, Cart cartService)
        {
            repository = repoService;
            cart = cartService;
        }

        #region Метод List()
        //Метод List() выбирает все объекты Order в хранилище, свойство Shipped кото­
        //рых имеет значение false , и передает их стандартному представлению.Этот метод
        //действия будет использоваться для отображения администратору списиа неотгружен­
        //ных заказов.
        #endregion
        #region Атрибут Authorize 
        // Атрибут Authorize применяется для ограничения доступа к методам действий. 
        // вешают на те контроллеры на которых нужно проводить аутентификацию пользователя.  что приведет 
        // к применению политики авторизации ко всем содержащимся в нем методам действий
        #endregion
        [Authorize]
        public ViewResult List() => View(repository.Orders.Where(o => !o.Shipped));

        #region MarkShipped
        // Метод MarkShipped() будет получать запрос POST, указывающий идентифика­
        // тор заказа, который применяется для извлечения соответствующего объекта Order
        // из хранилища, чтобы установить его свойство Shipped в true и сохранить.
        #endregion
        [HttpPost]
        [Authorize] // также защищаем все методы действий определяемые контроллером Admin
        public IActionResult MarkShipped(int orderID)
        {
            Order order = repository.Orders.FirstOrDefault(o => o.OrderID == orderID);
            if (order != null)
            {
                order.Shipped = true;
                repository.SaveOrder(order);
            }
            return RedirectToAction(nameof(List));
        }

        // Метод Checkout() возвращает стандартное представление и передает новый объ ­
        // ект ShippingDetails в качестве модели представления.
        public ViewResult Checkout() => View(new Order());

        #region Checkout
        //Метод дей ствия Checkout(} декорирован атрибутом HttpPost, т. е.он будет вы­
        //зываться для запроса POST - в этом случае, когда пользователь отправляет форму
        //( когда пользователь щелкает на кнопке Complete order (Завершить заказ),
        //Мы снова полагаемся на систему привязки моделей, так что можно получить объект
        //Order, дополнить его данными из объекта Cart и сохранить в хранилище . 
        #endregion
        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (cart.Lines.Count() == 0)
            {
                #region ModelState
                // Инфраструктура МVС контролирует огран ич е ния пров ерки достов е рности, кото­
                // рые были приме н е ны I < классу Order посредством атрибутов аннота ций данных, и
                // через свойство ModelState сообщает м етоду действия о любых пробл емах.Чтобы вы ­
                // яснить , есть ли пробл е мы, мы проверяем свойство ModelState . IsValid.Мы вызы­
                // ваем метод Mode l State.AddModelError() для р е гистрации сообщения об ошибке, 
                // есл и в корзине н ет элементов.Вскоре мы объясним , как отобр ажать таки е сообщения
                // об ошибках, а более подробное описание привязки модел ей и пров е рки достов е рности
                // будет представлено в гл авах 27 и 28.
                #endregion
                ModelState.AddModelError(" ", "Sorry your cart is empty");
            }
            if (ModelState.IsValid)
            {
                order.Lines = cart.Lines.ToArray();
                repository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            else { return View(order); }
        }

        public ViewResult Completed() // простая заглушка отображает Completed.cshtml
        {
            cart.Clear();
            return View();
        }

    }


}
