using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    // контроллер CRUD 
    // отдельный контроллер для управления каталогом товаров.
    [Authorize]
    public class AdminController : Controller
    {
        private IProductRepository repository;

        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index() => View(repository.Products);

        #region Edit
        //Edit будет получать НTТР-запрос, отправляемый браузером, когда пользо­
        //ватель щелкает на кнопке Edit.
        //Этот простой метод ищет товар с идентификатором, соответствующим значе ­
        //нию параметра productid, и передает его как объект модели представления методу
        //View() . 
        #endregion
        public ViewResult Edit(int productId) => View(repository
            .Products.FirstOrDefault(p => p.ProductID == productId));


        #region перегруженная версия метода действия Edit()
        // перегруженная версия метода действия Edit(), которая будет обрабатывать запросы POST, 
        // инициируемые по щелчку администратором на кнопке Save(Сохранить). 

        // Мы выясняем, смог ли процесс привязки модели проверить достоверность отправлен­
        // ных пользователем данных, для чего читаем значение свойства ModelState.IsValid.
        // Если здесь все в порядке, тогда мы сохраняем изменения в хранилище и направляем
        // пользователя на действие I ndex, таJ<что он увидит модифицированный список това­
        // ров.В случае какой-нибудь проблемы с данными мы снова визуализируем стандарт­
        // ное представление, чтобы пользователь мог внести 1юрректировки.
        // После сохранения изменений в хранилище сообщение сохраня ется с использо­
        // ванием средства TempDa t а, которое является частью средства состояния сеанса
        // ASP.NET Core.Это словарь пар "1шюч /знач ение", похожий на применяемые ранее
        // средства данных сеанса и ViewBag.Основное отличие объекта TernpData от данных
        // сеанса в том, что он хранится до тех пор, пока не будет прочитан.
        // В такой ситуации использовать ViewBag невозможно, потому что объект ViewBag
        // передает данные между контроллером и представлением, и он не может удерживать
        // данные дольше, чем длится текущий НТТР-запрос.Когда редактирование успешно, 
        // браузер перенаправляется на новый URL, поэтому данные ViewBag утрачиваются.
        // Мы могли бы прибегнуть к средству данных сеанса, но тогда сообщение хранилось
        // бы вплоть до его явного удаления, чего не хотелось бы делать.
        // Таким образом, объе1п TempData подходит ка~< нельзя лучше.Данные ограничи­
        // ваются сеансом одного пользователя (пользователи не видят объекты TempData друг
        // друга) и хранятся достаточно долго, чтобы быть прочитанными.Мы будем читать
        // данные в представлении , которое визуализируется методом действия, куда был пере­
        // направлен пользов атель, и определяется в следующем разделе.
        #endregion
        [HttpPost]
        public IActionResult Edit(Products product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = $"{product.Name} has been saved";
                return RedirectToAction("Index");
            }
            else
            {
                return View(product); // что то не так со значениями данных
            }
        }

        #region Create()
        // метод действия Create(), для кнопки Add
        // Product на главной странице со списком товаров. Он позволит администратору добав­
        // лять новые элементы в каталог товаров.
        // Метод Create() не визуализирует свое стандартное представление. Взамен он ука­
        // зывает, что должно использоваться представление "Edit".

        // Это единственное изменение, которое потребовалось, поскольку метод действия
        // Edit() уже настроен на получение объектов Product от системы привязки моделей
        // и на их сохранение в базе данных.
        #endregion
        public ViewResult Create() => View("Edit", new Products());

        #region Delete
        // 
        #endregion
        [HttpPost]
        public IActionResult Delete(int productId)
        {
            Products deletedProduct = repository.DeleteProducts(productId);
            if (deletedProduct != null)
            {
                TempData["message"] = $"{deletedProduct.Name} was deleted"; 
            }
            return RedirectToAction("Index");
        }

       
    }
}
