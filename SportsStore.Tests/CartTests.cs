using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using SportsStore.Models;
using Xunit;

//все тесты:
//Can_Add_New_Lines
//Can_Add_Quantity_For_Existing_Lines
//Can_Remove_Line
//Calculate_Cart_Total
//Can_Clear_Contents

namespace SportsStore.Tests
{
    public class CartTests
    {
        // проверка корзины 
        [Fact]
        public void Can_Add_New_Lines()
        {
            // создание пару тестовых товаров
            Products p1 = new Products { ProductID = 1, Name = "P1" };
            Products p2 = new Products { ProductID = 2, Name = "P2" };

            Cart target = new Cart(); // создаем новую корзину

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            Assert.Equal(2, results.Length); // сверка что в массиве должно быть 2 элимента (два товара)
            Assert.Equal(p1, results[0].Products); // сверка что товар p1 лежит в в массиве под первым (нулевым) индексом 
            Assert.Equal(p2, results[1].Products); // аналогично 
        }

        //Но если пользователь уже добавлял объект Product в корзину, тогда необходимо уве­
        //личить количество в соответствующем экземпляре CartLine, а не создавать новый. Вот
        //модульный тест: 

        [Fact]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            Products p1 = new Products { ProductID = 1, Name = "P1" };
            Products p2 = new Products { ProductID = 2, Name = "P2" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 10);
            CartLine[] results = target.Lines.OrderBy(c => c.Products.ProductID).ToArray();

            Assert.Equal(2, results.Length); //  
            Assert.Equal(11, results[0].Quantity); // сверка количества по p1 1+10=11  (провеяем что корзина увеличивается по заданному товару)
            Assert.Equal(1, results[1].Quantity); // аналогчно сверка кол-ва по p2 (по индекс массива)
        }

        // И нам также необходимо проверить, что пользователи имеют возможность менять свое
        // решение и удалять товары из корзины. Эта линия поведения реализуется методом
        // RemoveLine()

        [Fact]  
        public void Can_Remove_Line()
        {
            Products p1 = new Products { ProductID = 1, Name = "P1" };
            Products p2 = new Products { ProductID = 2, Name = "P2" };
            Products p3 = new Products { ProductID = 3, Name = "P3" };
            Products p4 = new Products { ProductID = 4, Name = "P4" };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 3);
            target.AddItem(p3, 5);
            target.AddItem(p2, 1);

            target.RemoveLine(p2);

            Assert.Equal(0, target.Lines.Where(c => c.Products == p2).Count()); // сверка что мы удалали товар p2 и count = 0 по p2
            Assert.Equal(2, target.Lines.Count()); // 2 - сверка сколько осталось товарных позиций 

        }
        // Далее проверяется линия поведения, связанная с возможностью вычисления общей стои­
        // мости элементов в корзине.

        [Fact]
        public void Calculate_Cart_Total()
        {
            Products p1 = new Products { ProductID = 1, Name = "P1", Price = 100m };
            Products p2 = new Products { ProductID = 2, Name = "P2", Price = 50m };

            Cart target = new Cart();

            target.AddItem(p1, 1); // 100m
            target.AddItem(p2, 1); // 50m
            target.AddItem(p1, 3); // 400m

            decimal result = target.ComputeTotalValue();

            Assert.Equal(450m, result); // сравниваем результат = 450
        }

        // Последний тест очень прост. Мы должны удостовериться , что в результате очистки корзины
        // ее содержимое корректно удаляется.

        [Fact]
        public void Can_Clear_Contents()
        {
            Products p1 = new Products { ProductID = 1, Name = "P1", Price = 100m };
            Products p2 = new Products { ProductID = 2, Name = "P2", Price = 50m };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);

            target.Clear();

            Assert.Equal(0, target.Lines.Count());
        }
        
        //---------------------------------

        // Временами , как в данном случае, код для тестирования функциональности класса получает­
        // ся намного длиннее и сложнее, чем код самого класса. Не допускайте, чтобы это приводило
        // к отказу от написания модульных тестов. Дефекты в простых классах, особенно в тех, кото­
        // рые играют настолько важную роль, как Cart в приложении SportsStore, могут оказывать
        // разрушительное воздействие. 

    }
}
