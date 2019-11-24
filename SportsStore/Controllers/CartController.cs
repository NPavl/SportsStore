using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportsStore.Infrastructure;

namespace SportsStore.Controllers
{
    #region описание контроллера Cart
    //Для сохранения и извлечения объектов Cart применяется средство состояния сеанса
    //ASP.NET, с которым и взаимодействует метод GetCart(). Промежуточное програм­
    //мное обеспечение, зарегистрированное в предыдущем разделе (в классе SturtUp 
    //(services.AddМemoryCache() services.AddSession() app.UseSession()) использует сооkiе­
    //наборы или переписывание URL, чтобы ассоциировать вместе множество запросов
    //от определенного пользователя с целью формирования отдельного сеанса просмотра . 
    //Связанным средством является состояние сеанса, которое ассоциирует данные с сеан­
    //сом. Это идеально подходит для класса Cart: мы хотим, чтобы каждый пользователь
    //имел собственную корзину, и эта корзина сохранялась между запросами. Данные
    //связанные с сеансом, удаляются по истечении времени существования сеанса (обыч­
    //но из-за того, что пользователь не отправляет запрос какое-то время) , т.е. управлять
    //хранилищем или жизненным циклом объектов Cart не придется.

    //В методах действий AddToCart() и RemoveFromCart() применялись имена пара ­
    //метров, которые соответствуют именам элементов input в НТМL-формах , созданных
    //в представлении ProductSummary.cshtml.Это позволяет инфраструктуре МVС ассо­
    //циировать входящие переменные НТТР-запроса POST формы с параметрами и означа ­
    //ет, что делать что-то самостоятельно для обработки формы не нужно.Такой процесс
    //называется привязкой модели и с его помощью можно значительно упрощать классы
    //контроллеров, как будет объясняться в главе 26. 

    #endregion

    #region описание конроллера Cart в новой облегченной версии 
    // Класс CartController указывает на то, что он нуждается в объекте Cart, за счет
    // объявления аргумента конструктора.Это позволяет удалить методы, которые читают
    // и записывают данные в сеанс, а также код, требующийся для записи обновлений. 
    // Результатом явля ется контроллер, который не только проще, но и более сосредоточен
    // на своей роли в приложении, не беспокоясь о том, как объекты Cart создаются или
    // хранятся. И поскольку службы доступны по всему приложению, любой компонент мо­
    // жет получать корзину пользователя с применением одного и того же приема. 
    #endregion

    public class CartController : Controller
    {
        private IProductRepository repository;
        private Cart cart;

        public CartController(IProductRepository repo, Cart cartService)
        {
            repository = repo;
            cart = cartService;
        }

        #region Index
        //метод Index() будет применяться для отображения со­держимого объекта Cart.

        //Действие Index извлекает объект Cart из состояния сеанса и использует его для
        //создания обьекта CartindexViewModel, который затем передается методу View()
        //для применения в качестве модели представления.
        #endregion
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                //Сart = GetCart(), исключен в прощенной версии CartController
                Сart = cart,
                ReturnUrl = returnUrl
            });
        }

        #region RedirectToActionResult описание класса
        // Microsoft.AspNetCore.Mvc.ActionResult, который возвращает найденное(302), перемещенное навсегда
        // (301), временное перенаправление (307) или постоянное перенаправление (308) с ответом
        // Заголовок местоположения. Целевые действия контроллера.
        #endregion
        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Products products = repository.Products
                .FirstOrDefault(p => p.ProductID == productId); // FirstOrDefault -Returns the first element of the sequence that satisfies a condition or a default
                                                                // value if no such element is found.
            if (products != null)
            {
                //Cart cart = GetCart(); исключен в прощенной версии CartController
                cart.AddItem(products, 1); 
                //SaveCart(cart); исключен в прощенной версии CartController
            }
            #region RedirectToAction описание 
            //Финальное замечание о контроллере Cart касается того, что методы AddToCart()
            //и RemoveFromCart() вызывают метод RedirectToAction(). Результатом будет от­
            //правка клиентскому браузеру НТTР-инструкции перенаправления, которая заставит
            //браузер запросить новый URL. В данном случае браузер запросит URL, который вы­
            //зывает метод действия Index() контроллера Cart. 
            #endregion
            return RedirectToAction("Index", new { returnUrl });
        }
        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            Products products = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);
            if (products != null)
            {
                //Cart cart = GetCart();  исключен в прощенной версии CartController
                cart.RemoveLine(products);
                //SaveCart(cart);  исключен в прощенной версии CartController
            }
            #region RedirectToAction описание 
            //Финальное замечание о контроллере Cart касается того, что методы AddToCart()
            //и RemoveFromCart() вызывают метод RedirectToAction().Результатом будет от­
            //правка клиентскому браузеру НТГР-инструкции перенаправления, которая заставит
            //браузер запросить новый URL. В данном случае браузер запросит URL, который вы­
            //зывает метод действия Index() контроллера Cart. 
            #endregion
            return RedirectToAction("Index", new { returnUrl });
        }

        #region HttpContext.Session (SetJson и GetJson)
        //Свойство HttpContext определено в базовом классе Controller, от которого
        //обычно унаследованы контроллеры, и возвращает объект HttpContext. Этот объект
        //предоставляет данные контекста о запросе, который был получен, и ответе, находя­
        //щемся в процессе подготовки. Свойство HttpContext.Session возвращает объект, 
        //реализующий интерфейс ISession. Данный интерфейс является именно тем типом,
        //где мы определили метод SetJson(), принимающий аргументы, в которых указы-
        //ваются ключ и объект, подлежащий добавлению в состояние сеанса. Расширяющий
        //метод сериализирует объект и добавляет его в состояние сеанса, используя функцио­
        //нальность, которая лежит в основе интерфейса ISession.
        //Чтобы извлечь объект Cart, применяется другой расширяющий метод, которому
        //указывается тот же самый ключ: Cart cart = HttpContext.Session.GetJson<Cart>(Cart);
        //Параметр типа позволяет задать тип объекта, который ожидается извлечь; этот
        //тип используется в процессе десериализации.
        #endregion
        //private Cart GetCart() // удален в новой версии 
        //{
        //    Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
        //    return cart;
        //}
        //private void SaveCart(Cart cart)
        //{
        //    HttpContext.Session.SetJson("Cart", cart);
        //}
    }
}
//-----------------------------------------------
#region первоначальный вид класса CartController   

//public class CartController : Controller
//{
//    private IProductRepository repository;
//    //private Cart cart;

//    public CartController(IProductRepository repo)
//    {
//        repository = repo;
//        //cart = cartService;
//    }

//    #region Index
//    //метод Index() будет применяться для отображения со­держимого объекта Cart.

//    //Действие Index извлекает объект Cart из состояния сеанса и использует его для
//    //создания обьекта CartindexViewModel, который затем передается методу View()
//    //для применения в качестве модели представления.
//    #endregion
//    public ViewResult Index(string returnUrl)
//    {
//        return View(new CartIndexViewModel
//        {
//            Сart = GetCart(),
//            ReturnUrl = returnUrl
//        });
//    }

//    #region RedirectToActionResult описание класса
//    // Microsoft.AspNetCore.Mvc.ActionResult, который возвращает найденное(302), перемещенное навсегда
//    // (301), временное перенаправление (307) или постоянное перенаправление (308) с ответом
//    // Заголовок местоположения. Целевые действия контроллера.
//    #endregion
//    public RedirectToActionResult AddToCart(int productId, string returnUrl)
//    {
//        Products products = repository.Products
//            .FirstOrDefault(p => p.ProductID == productId); // FirstOrDefault -Returns the first element of the sequence that satisfies a condition or a default
//                                                            // value if no such element is found.
//        if (products != null)
//        {
//            Cart cart = GetCart();
//            cart.AddItem(products, 1);
//            SaveCart(cart);
//        }
//        #region RedirectToAction описание 
//        //Финальное замечание о контроллере Cart касается того, что методы AddToCart()
//        //и RemoveFromCart() вызывают метод RedirectToAction(). Результатом будет от­
//        //правка клиентскому браузеру НТTР-инструкции перенаправления, которая заставит
//        //браузер запросить новый URL. В данном случае браузер запросит URL, который вы­
//        //зывает метод действия Index() контроллера Cart. 
//        #endregion
//        return RedirectToAction("Index", new { returnUrl });
//    }
//    public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
//    {
//        Products products = repository.Products
//            .FirstOrDefault(p => p.ProductID == productId);
//        if (products != null)
//        {
//            Cart cart = GetCart();
//            cart.RemoveLine(products);
//            SaveCart(cart);
//        }
//        #region RedirectToAction описание 
//        //Финальное замечание о контроллере Cart касается того, что методы AddToCart()
//        //и RemoveFromCart() вызывают метод RedirectToAction().Результатом будет от­
//        //правка клиентскому браузеру НТГР-инструкции перенаправления, которая заставит
//        //браузер запросить новый URL. В данном случае браузер запросит URL, который вы­
//        //зывает метод действия Index() контроллера Cart. 
//        #endregion
//        return RedirectToAction("Index", new { returnUrl });
//    }

//    #region HttpContext.Session (SetJson и GetJson)
//    //Свойство HttpContext определено в базовом классе Controller, от которого
//    //обычно унаследованы контроллеры, и возвращает объект HttpContext. Этот объект
//    //предоставляет данные контекста о запросе, который был получен, и ответе, находя­
//    //щемся в процессе подготовки. Свойство HttpContext.Session возвращает объект, 
//    //реализующий интерфейс ISession. Данный интерфейс является именно тем типом,
//    //где мы определили метод SetJson(), принимающий аргументы, в которых указы-
//    //ваются ключ и объект, подлежащий добавлению в состояние сеанса. Расширяющий
//    //метод сериализирует объект и добавляет его в состояние сеанса, используя функцио­
//    //нальность, которая лежит в основе интерфейса ISession.
//    //Чтобы извлечь объект Cart, применяется другой расширяющий метод, которому
//    //указывается тот же самый ключ: Cart cart = HttpContext.Session.GetJson<Cart>(Cart);
//    //Параметр типа позволяет задать тип объекта, который ожидается извлечь; этот
//    //тип используется в процессе десериализации.
//    #endregion
//    private Cart GetCart()
//    {
//        Cart cart = HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart();
//        return cart;
//    }
//    private void SaveCart(Cart cart)
//    {
//        HttpContext.Session.SetJson("Cart", cart);
//    }
//}
//}
#endregion
//-----------------------------------------------