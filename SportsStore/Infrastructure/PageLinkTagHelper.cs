using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Infrastructure
{
    //Дескрипторный вспомогательный класс
    //В папку Infrastructure будут помещаться классы, которые предоставляют прило­
    //жению связующий код, но не имеют отношения к предметной области приложения.

    //Этот дескрипторный вспомогательный класс заполняет элемент div элементами
    //- а, которые соответствуют страницам товаров. Сейчас мы не будем вдаваться в ка­
    //кие-либо детали относительно дескрипторных вспомогательных классов; достаточно
    //знать, что они предлагают один из наиболее удобных способов помещения логики С# 
    //в представления. Код для дескрипторного вспомогательного класса может выглядеть
    //запутанным, потому что смешивать С# и HTML непросто. Но использование дескрип­
    //торных вспомогательных классов является предпочтительным способом включения
    //блоков кода С# в представление, поскольку дескрипторный вспомогательный класс 
    //можно легко подвергать модульному тестированию.

    //    Большинство компонентов MVC, таких как контроллеры и представления , об­
    //наруживаются автоматически, но дескрипторные вспомогательные классы долж­
    //ны быть зарегистрированы. В листинге 8.26 приведено содержимое файла
    //_ Viewimports.cshtml из папки Views с добавленным оператором, который сооб­
    //щает MVC о том, что дескрипторные вспомогательные классы следует искать в про­
    //странстве имен SportsStore.Infrastructure. Кроме того, добавлено также вы­
    //ражение @using, чтобы на классы моделей представлений можно было ссылаться в
    //представлениях, не указывая пространство имен .
    //-------------------------------------------------------------------------------

    //HtmlTargetElementAttribute - Создает новый экземпляр класса HtmlTargetElementAttribute, 
    //который предназначается для всех элементов HTML с необходимыми Атрибутами (div)
    //Атрибут (String) Создает новый экземпляр класса HtmlTargetElementAttribute с заданным тегом (div) в качестве значения тега.

    //TagHelper - абстрактный класс наследуемый от интерфейса ITagHelper

    //ViewContext Контекст для представления представления.
    //ViewContextAttribute Указывает, что свойство помощника тега должно быть установлено с 
    //текущим ViewContext при создании помощника тега. Свойство должно иметь открытый метод set.

    //HtmlAttributeNotBoundAttribute - Создает новый экземпляр класса HtmlAttributeNotBoundAttribute.

    //TagBuilder Содержит методы и свойства, которые используются для создания элементов HTML. 
    //Этот класс часто используется для написания HTML-помощников и тегов-помощников.

    //IUrlHelper Определяет контракт для помощника по созданию URL-адресов для ASP.NET MVC в приложении.
    //IUrlHelperFactory - Фабрика для создания экземпляров IUrlHelper.
    //GetUrlHelper Получает IUrlHelper для запроса, связанного с контекстом.

    //PageLinkTagHelper Дескрипторный вспомогательный класс

    //TagHelperContext  Содержит информацию, связанную с выполнением ITagHelpers.
    //ITagHelper - Контракт, используемый для фильтрации соответствующих элементов HTML. Маркерный интерфейс для TagHelpers.
    //TagHelperOutput Класс, используемый для представления вывода ITagHelper.

    //AppendHtml - Добавляет htmlContent к существующему контенту.
    //InnerHtml - Получает внутреннее содержимое HTML элемента.

    [HtmlTargetElement("div", Attributes = "page-model")] // Когда механизм Razor обнаружит атрибут page-model в элементе div, он обращается к классу PageLinkTagHelper для пре­образования элемента, что и дает набор ссьлок.
    public class PageLinkTagHelper : TagHelper // TagHelper - абстрактный класс наследуемый от интерфейса 
                                               // ITagHelper - Контракт, используемый для фильтрации соответствующих 
                                               // элементов HTML. Маркерный интерфейс для TagHelpers.
    {

        private IUrlHelperFactory urlHelperFactory; // IUrlHelperFactory - Фабрика для создания экземпляров IUrlHelper.
        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            urlHelperFactory = helperFactory;
        }
        [ViewContext]  //ViewContext Контекст для представления представления.
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; } //ViewContext Контекст для представления представления.
        public PagingInfo PageModel { get; set; } //PagingInfo класс модели представления
        public string PageAction { get; set; }

        //Получение значений атрибутов, снабженных префиксом DictionaryAttributePrefix. 
        //получать в одной коллекции все свойства с общим префиксом.
        //Декорирование свойства дескрипторного вспомогательного класса посредс­
        //твом атрибута HtmlAttributeName позволяет указывать префикс для имен атри­
        //бутов элемента, которым в данном случае будет page-url-. Значение любого ат­
        //рибута, имя которого начинается с такого префикса, будет добавлено в словарь,
        //присваиваемый свойству РageUrlVаlues. Затем это свойство передается методу
        //IUrlHelper.Action() с целью генерации URL для атрибута href элементов а , ко­
        //торые производит дескрипторный вспомогательный класс.
        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; }
        = new Dictionary<string, object>();

        public bool PageClassesEnabled { get; set; } = false;
        public string PageClass { get; set; }
        public string PageClassNormal { get; set; }
        public string PageClassSelected { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //Интерфейс IUrlHelper предоставляет доступ к функциональности генерации URL.
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext); // IUrlHelper Определяет контракт для помощника по созданию URL-адресов для ASP.NET MVC в приложении.
            TagBuilder result = new TagBuilder("div"); // TagBuilder Содержит методы и свойства, которые используются для создания элементов HTML. 
            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                PageUrlValues["page"] = i;
                tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                //  tag.Attributes["href"] = urlHelper.Action(PageAction, new { page = i });
                if (PageClassesEnabled)
                {
                    tag.AddCssClass(PageClass);
                    tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
                }
                tag.InnerHtml.Append(i.ToString()); //Append Добавляет строковое значение. Значение обрабатывается как не закодированное как предоставлено 
                                                    //и будет закодировано в HTML перед записью в вывод.
                result.InnerHtml.AppendHtml(tag);  //InnerHtml - Получает внутреннее содержимое HTML элемента.
            }
            output.Content.AppendHtml(result.InnerHtml); //AppendHtml - Добавляет htmlContent к существующему контенту.
        }
    }
}
