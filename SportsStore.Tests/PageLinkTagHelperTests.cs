using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SportsStore.Infrastructure;
using SportsStore.Models.ViewModels;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace SportsStore.Tests
{
    //Сложность этого теста связана с созданием объектов, которые требуются для создания и
    //применения дескрипторного вспомогательного класса. Дескрипторные вспомогательные
    //классы используют объекты IUrlHelperFactory для генерации URL, которые указывают
    //на разные части приложения, и для создания реализаций этого интерфейса и связанного
    //с ним интерфейса IUrlHelper, предоставляющего тестовые данные, используется инф­
    //раструктура Moq.
    //Основная часть теста проверяет вывод дескрипторного вспомогательного класса с приме­
    //нением литерального строкового значения, которое содержит двойные кавычки. Язык С# 
    //позволяет работать с такими строками при условии, что строка предварена символом @, а
    //вместо одной двойной кавычки внутри строки используется набор из двух двойных кавычек
    //( """" ) . Вы должны помнить о том , что разносить литеральную строку по нескольким строкам
    //файла нельзя , если только строка , с которой производится сравнение, не разнесена ана­
    //логичным образом. Например, литерал, применяемый в тестовом методе, был размещен в
    //нескольких строках из-за недостаточной ширины печатной страницы. Символ новой строки
    //не добавлялся, иначе тест не прошел бы.

    public class PageLinkTagHelperTests
    {

        #region описание интерфейсов методов и классов:

        //IUrlHelper Определяет контракт для помощника по созданию URL-адресов для ASP.NET MVC в приложении.
        //IUrlHelperFactory - Фабрика для создания экземпляров IUrlHelper.
        //GetUrlHelper Получает IUrlHelper для запроса, связанного с контекстом.

        //Action - Создает URL-адрес с абсолютным путем для метода действия, который содержит имя действия, 
        //имя контроллера, значения маршрута, используемый протокол, имя хоста и фрагмент, указанный в UrlActionContext. 
        //Создает абсолютный URL, если Protocol и Host не равны NULL. См. Раздел замечаний для важной информации о безопасности.

        //UrlActionContext Объект контекста, который будет использоваться для URL-адресов, которые генерирует Action (UrlActionContext).

        //PageLinkTagHelper Дескрипторный вспомогательный класс

        //PagingInfo класс модели представления

        //TagHelperContext  Содержит информацию, связанную с выполнением ITagHelpers.
        //ITagHelper - Контракт, используемый для фильтрации соответствующих элементов HTML. Маркерный интерфейс для TagHelpers.

        //TagHelperAttributeList Создает новый экземпляр TagHelperAttributeList с пустой коллекцией.

        //TagHelperContent - Абстрактный класс, используемый для буферизации содержимого, возвращаемого ITagHelpers.

        //TagHelperOutput - Класс, используемый для представления вывода ITagHelper.

        #endregion
        [Fact]
        public void Can_Generate_Page_Links()
        {
            var urlHelper = new Mock<IUrlHelper>(); //IUrlHelper Определяет контракт для помощника по созданию URL-адресов для ASP.NET MVC в приложении.
            urlHelper.SetupSequence(x => x.Action(It.IsAny<UrlActionContext>()))  //Action - Создает URL-адрес с любым абсолютным путем для метода действия, который содержит имя действия....
                .Returns("Test/Page1") //UrlActionContext Объект контекста, который будет использоваться для URL-адресов, которые генерирует Action (UrlActionContext).
                .Returns("Test/Page2")
                .Returns("Test/Page3");

            var urlHelperFactory = new Mock<IUrlHelperFactory>();  //IUrlHelperFactory - Фабрика для создания экземпляров IUrlHelper.
            urlHelperFactory.Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>())) //GetUrlHelper Получает IUrlHelper для запроса, связанного с контекстом.
            .Returns(urlHelper.Object);
            PageLinkTagHelper helper = new PageLinkTagHelper(urlHelperFactory.Object)  //PageLinkTagHelper Дескрипторный вспомогательный класс
            {
                PageModel = new PagingInfo //PagingInfo класс модели представления
                {
                    CurrentPage = 2,
                    TotalItems = 28,
                    ItemsPerPage = 10
                },
                PageAction = "Test"
            };  
            TagHelperContext ctx = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), "");
            //TagHelperContent - Абстрактный класс, используемый для буферизации содержимого, возвращаемого ITagHelpers.
            //TagHelperAttributeList Создает новый экземпляр TagHelperAttributeList с пустой коллекцией.

            var content = new Mock<TagHelperContent>(); 

            TagHelperOutput output = new TagHelperOutput("div", new TagHelperAttributeList(), (cache, encoder) => Task.FromResult(content.Object));

            helper.Process(ctx, output);

            // public static void Equal(string expected, string actual);
            Assert.Equal(@"<a href=""Test/Page1"">1</a>"
            + @"<a href=""Test/Page2"">2</a>"
            + @"<a href=""Test/Page3"">3</a>",
            output.Content.GetContent());
        }

        //IUrlHelper 
    }
}

