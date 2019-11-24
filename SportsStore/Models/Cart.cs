using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    // класс для работы с корзиной покупок 
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public virtual void AddItem(Products product, int quantity)
        {
            CartLine line = lineCollection
                .Where(p => p.Products.ProductID == product.ProductID)
                .FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Products = product,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public virtual void RemoveLine(Products product) => lineCollection.RemoveAll(l => l.Products.ProductID
           == product.ProductID);

        public virtual decimal ComputeTotalValue() => lineCollection.Sum(e => e.Products.Price * e.Quantity);

        public virtual void Clear() => lineCollection.Clear();

        public virtual IEnumerable<CartLine> Lines => lineCollection;

        // Класс Cart использует класс CartLine, который определен в том же самом файле
        // и представляет товар, выбранный пользователем, а также приобретаемое его коли­
        // чество. Мы определили методы для добавления элемента в корзину, удаления элемен ­
        // та из корзины, вычисления общей стоимости элементов в корзине и очистки корзины
        // путем удаления всех элементов. Мы также предоставили свойство, которое позволяет
        // обратиться к содержимому корзины с применением IEnumeraЬle<CartLine>. Все
        // это легко реализуется с помощью кода С# и небольшой доли кода LINQ. 
    }
    // вспомогательный для каласса Car класс CartLine
    public class CartLine
    {
        public int CartLineID { get; set; }
        public Products Products { get; set; }
        public int Quantity { get; set; }
    }
}
