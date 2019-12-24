using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
  
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

       
    }

    public class CartLine
    {
        public int CartLineID { get; set; }
        public Products Products { get; set; }
        public int Quantity { get; set; }
    }
}
