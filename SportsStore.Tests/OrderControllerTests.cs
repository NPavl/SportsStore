using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        // суть теста: необходимо проверить поведение версии POST метода Checkout() . Хотя этот метод выглядит коротким и
        // простым, использование привязки моделей MVC означает наличие многих вещей, происхо­
        // дящих "за кулисами", которые должны быть протестированы.
        // Мы хотим обрабатывать заказ , только если в корзине присутствуют элементы, и поль­
        // зователь предоставил достоверные детали о доставке . При любых других обстоятельс ­
        // твах пользователю должно быть сообщено об ошибке.

        // Тест проверяет отсутствие возможности перехода к оплате при пустой корзине.Мы удосто­
        // веряе м ся, что метод SaveOrder () и митированной реализации IOrderRepos i tory н и ­
        // к огда не вызывается, что метод возвращает стандартное представлен и е (к оторое повторно
        // отобразит введенные пользователем данные, давая ему шанс откорре к тировать их) и что
        // состояние модели, передаваемое представлению, помечено ка к недопустимое.Это мо жет
        // выглядеть ка к и злишн е ограничивающий набор утверждений, но для провер к и правильности
        // поведения нуж ны все три утвер ждения.


        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();

            Order order = new Order();

            OrderController target = new OrderController(mock.Object, cart);

            ViewResult result = target.Checkout(order) as ViewResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            Assert.True(string.IsNullOrEmpty(result.ViewName));

            Assert.False(result.ViewData.ModelState.IsValid);

        }

        // Следующий тестовый метод работает в основном
        // так же , но внедряет в модель представления ошибку, эмулирующую проблему, о которой
        // сообщает средство привяз к и модели(что должно происходить в производственной среде,
        // когда пользователь вводит некорректные данные о доставке): 

        [Fact]

        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();

            cart.AddItem(new Products(), 1);

            OrderController target = new OrderController(mock.Object, cart);

            target.ModelState.AddModelError("error", "error");

            ViewResult result = target.Checkout(new Order()) as ViewResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);

            Assert.False(result.ViewData.ModelState.IsValid);

        }

        // Удостоверившись в том, что пустая корзина или некорректные данные о доставке предо­
        // твращают сохранение заказа , необходимо проверить, что нормальные заказы сохраняются
        // должным образом. Ниже приведен тест.

        [Fact]

        public void Can_Checkout_And_Submit_Order()
        {
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            Cart cart = new Cart();

            cart.AddItem(new Products(), 1);

            OrderController target = new OrderController(mock.Object, cart);

            RedirectToActionResult result = target.Checkout(new Order()) as RedirectToActionResult;

            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);

            Assert.Equal("Completed", result.ActionName);
        }

        // Тестировать возможность идентификации допустимых сведений о доставке не нужно. Это
        // автоматически обрабатывается средством привязки моделей с использованием атрибутов,
        // примененных к свойствам класса Order. 

    }
}
