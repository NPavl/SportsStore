using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.AspNetCore.Routing;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

// Can_Select_Categories
// Indicates_Selected_Category()


namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        // Создаем имитированную реализацию хранилища, которая содержит повторяющиеся и
        // несортированные категории. Затем мы устанавливаем утверждение о том , что дубликаты
        // удалены и восстановлен алфавитный порядок.
        [Fact]
        public void Can_Select_Categories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Products[]
                {
                    new Products { ProductID=1, Name="P1", Category="Apples"},
                    new Products { ProductID=2, Name="P2", Category="Apples"},
                    new Products { ProductID=3, Name="P3", Category="Plums"},
                    new Products { ProductID=4, Name="P4", Category="Oranges"},
                });

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            //создаем экз класса NavigationMenuViewComponent для вызова не нем метода Invoke 

            string[] results = ((IEnumerable<string>)(target.Invoke() as ViewViewComponentResult).ViewData.Model).ToArray();
            //как я понял при вызове метода Invoke на экз класса NavigationMenuViewComponent , Invoke вернет нам уже 
            //отсортированную с удаленными дубликатами категории и ниже мы проверяем это утверждение.

            Assert.True(Enumerable.SequenceEqual(new string[] { "Apples", "Oranges", "Plums" }, results));
            // создаем массив отосртированных категорий и сверяем с тем которые нам отсортировал  вернул метод Invoke.
        }
        //-------------------------------------------------------------------------------------
        // СООБЩЕНИЕ О ВЫБРАННОЙ КАТЕГОРИИ 
        // Для выполнения проверки того, что компонент представления корректно добавил детали о
        // выбранной категории, в модульном тесте можно прочитать значение свойства ViewBag, 
        // которое доступно через класс ViewViewComponentResult, описанный в главе 22

        // Этот модульный тест снабжает компонент представления данными маршрутизации через
        // свойство ViewComponentContext , посредством которого компоненты представлений
        // получают все свои данные контекста.Свойство ViewComponentContext предоставляет
        // доступ к данным контекста, специфичным для представления, с помощью своего свойства
        // ViewContext, которое, в свою очередь, обеспечивает доступ к информации о маршрути­
        // зации через свое свойство RouteData.Большая часть кода в модульном тесте связана с
        // созданием объектов контекста, которые будут предоставлять выбранную категорию таким
        // же способом, как она бы предлагалась во время выполнения приложения, когда данные кон­
        // текста предоставляются инфраструктурой ASP.NET Саге MVC.
        [Fact]
        public void Indicates_Selected_Category()
        {
            string categoryToSelect = "Apples";
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Products[]
            {
                new Products {ProductID = 1, Name = "P1", Category = "Apples" },
                new Products {ProductID = 4, Name = "P2", Category = "Oranges" },
            });

            NavigationMenuViewComponent target = new NavigationMenuViewComponent(mock.Object);
            target.ViewComponentContext = new ViewComponentContext // class ViewComponent - A base class for view components.
            {
                ViewContext = new ViewContext { RouteData = new RouteData() } //класс RouteData- Information about the current routing path.
            }; // ViewContext - Context for view execution.(Контекст для представления представления)


            target.RouteData.Values["category"] = categoryToSelect;

            string result = (string)(target.Invoke() as ViewViewComponentResult).ViewData["SelectedCategory"];
            // класс ViewViewComponentResult отображает частичное представление, когда выполнено                                                                                                               выполнено.

            Assert.Equal(categoryToSelect, result);
        }

    }
}
