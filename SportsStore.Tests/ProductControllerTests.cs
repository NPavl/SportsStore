using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SportsStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

// ВСЕ ТЕСТЫ 
//Can_Paginate
//Can_Send_Pagination_View_Model
//Can_Filter_Products

namespace SportsStore.Tests
{
    // Модульное тестирование средства разбиения на страницы
    #region описания теста
    // Модульное тестирование средства разбиения на страницы можно провести, создав
    // имитированное хранилище, внедрив его в конструктор класса ProductController
    // и вызвав метод List() , чтобы запрашивать конкретную страницу. Затем полученные
    // объекты Products можно сравнить с теми, которые ожидались от тестовых данных в ими­
    // тированной реализации.

    // Получение данных, возвращаемых из метода действия, выглядит несколько неуклюже.
    // Результатом является объект ViewResult, и значение его свойства ViewData.Model
    // должно быть приведено к ожидаемому типу данных.
    #endregion
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Products[]
                {
                new Products {ProductID = 1, Name = "P1" },
                new Products {ProductID = 2, Name = "P2" },
                new Products {ProductID = 3, Name = "P3" },
                new Products {ProductID = 4, Name = "P4" },
                new Products {ProductID = 5, Name = "P5" }
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Действие
            //IEnumerable<Products> result = controller.List(2).ViewData.Model as IEnumerable<Products>;
            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;
            // 2 - это page как текущая страница тоесть вторая страница (устанавливается в метод List) по этому 
            // далее в тестах мы методом Equel сравнивам два Name P4 и P5 как последняя страница третья, 
            // тем самым мы запрашиваем конкретную страницу (в данно тесте вторую) и сверяем полученные данные 
            // из мока. 
            // Указывая null для category - List(null, 2), мы получаем все объекты Products, которые контрол­
            // лер извлекает из хранилища, что воспроизводит ситуацию перед добавлением ново ­
            // го параметра.

            //ViewData представляет словарь из пар ключ-значение
            //Утверждение
            Products[] prodArray = result.Products.ToArray(); // в этот массив prodArray (как вторая страница) 
            //будет оствшиеся P4 и P5 так как на первой сранице с PageSize = 3 она получит P1 P2 P3 

            Assert.True(prodArray.Length == 2); // сверка кол-ва элиментов в массиве должнобыть 2 ProductID 4 и 5
            Assert.Equal("P4", prodArray[0].Name); // итоговая сверка имен первого и второго элимента массива
            Assert.Equal("P5", prodArray[1].Name);

        }

        #region Can_Send_Pagination_View_Model
        //Нам необходимо удостовериться в том, что контроллер отправляет представлению коррек­
        //тную инфор мацию о разбиении на страницы. Ни же показан модульный тест, добавленный в
        //класс ProductControllerTests внутри тестового проекта, который обеспечивает такую
        //проверку: 
        #endregion
        //-----------------------------------------------------
        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Products[]
                {
                new Products {ProductID = 1, Name = "P1" },
                new Products {ProductID = 2, Name = "P2" },
                new Products {ProductID = 3, Name = "P3" },
                new Products {ProductID = 4, Name = "P4" },
                new Products {ProductID = 5, Name = "P5" }
            });

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Действие
            ProductsListViewModel result = controller.List(null, 2).ViewData.Model as ProductsListViewModel;
            // также устанавливаем текщую сраницу как вторую в метод List 

            //Утверждение
            PagingInfo pageInfo = result.PagingInfo; // сверка полученных данных их класса PagingInfo
            Assert.Equal(2, pageInfo.CurrentPage); // текущая страница должна быть вторая так как мы установили List(2) 
            Assert.Equal(3, pageInfo.ItemsPerPage); // свойство PageSize = 3 сверка кол-ва отображамыз товаров на странице 
            Assert.Equal(5, pageInfo.TotalItems); // сверка общего кол-ва товаров - 5
            Assert.Equal(2, pageInfo.TotalPages); // сверка общего кол-ва страниц - 2

        }
        //-----------------------------------------------------
        [Fact]
        public void Can_Filter_Products()
        {
            #region описание 
            //Этот тест создает имитированное хранилище, содержащее объекты Products, которые от­
            //носятся к диапазону категорий. С использованием метода действия List() запрашивается
            //одна специфическая категория, а результаты проверяются на предмет наличия корректных
            //объектов в правильном порядке. 
            #endregion

            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Products[]
                {
                    new Products { ProductID=1, Name="P1", Category="Cat1"},
                    new Products { ProductID=2, Name="P2", Category="Cat2"},
                    new Products { ProductID=3, Name="P3", Category="Cat1"},
                    new Products { ProductID=4, Name="P4", Category="Cat2"},
                    new Products { ProductID=5, Name="P5", Category="Cat3"},
                });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            Products[] result =
                (controller.List("Cat2", 1).ViewData.Model as ProductsListViewModel).Products.ToArray();

            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2"); // сверка категории и его поля Name по порядку P2-Cat2 P4-Cat2 
            Assert.True(result[1].Name == "P4" && result[0].Category == "Cat2");

        }
        //-------------------------------------------------------------------------

        // счетчик товаров определенной категории

        //Протестировать возможность генерации корректных счетчиков товаров для различных
        //категорий очень просто. Мы создадим имитированное хранилище, которое содержит из­
        //вестные данные в определенном диапазоне категорий, и затем вызовем метод действия
        //List(), запрашивая каждую категорию по очереди.

        //в тесте также вызывается метод List() без указания
        //категории, чтобы удостовериться в правильности подсчета общего количества товаров.

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Products[]
                {
                    new Products { ProductID=1, Name="P1", Category="Cat1"}, 
                    new Products { ProductID=2, Name="P2", Category="Cat2"},
                    new Products { ProductID=3, Name="P3", Category="Cat1"},
                    new Products { ProductID=4, Name="P4", Category="Cat2"},
                    new Products { ProductID=5, Name="P5", Category="Cat3"},
                });

            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3; 

            Func<ViewResult, ProductsListViewModel> GetModel = result1 => result1?.ViewData?.Model as ProductsListViewModel;

            int? res1 = GetModel(target.List("Cat1"))?.PagingInfo.TotalItems; 
            int? res2 = GetModel(target.List("Cat2"))?.PagingInfo.TotalItems;
            int? res3 = GetModel(target.List("Cat3"))?.PagingInfo.TotalItems;
            int? resAll = GetModel(target.List(null))?.PagingInfo.TotalItems;

            Assert.Equal(2, res1); // Cat1 должно быть 2 категории
            Assert.Equal(2, res2); // Cat2 должно быть 2 категории
            Assert.Equal(1, res3); // Cat1 должно быть 1 категории
            Assert.Equal(5, resAll); // всего категорий должно быть 5 

        }

    }
}