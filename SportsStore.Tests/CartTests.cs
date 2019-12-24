using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using SportsStore.Models;
using Xunit;


namespace SportsStore.Tests
{
    public class CartTests
    {
      
        [Fact]
        public void Can_Add_New_Lines()
        {
           
            Products p1 = new Products { ProductID = 1, Name = "P1" };
            Products p2 = new Products { ProductID = 2, Name = "P2" };

            Cart target = new Cart(); 

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            CartLine[] results = target.Lines.ToArray();

            Assert.Equal(2, results.Length);
            Assert.Equal(p1, results[0].Products);
            Assert.Equal(p2, results[1].Products); 
        }


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

            Assert.Equal(2, results.Length); 
            Assert.Equal(11, results[0].Quantity); 
            Assert.Equal(1, results[1].Quantity); 
        }

      
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

            Assert.Equal(0, target.Lines.Where(c => c.Products == p2).Count()); 
            Assert.Equal(2, target.Lines.Count()); 

        }
        

        [Fact]
        public void Calculate_Cart_Total()
        {
            Products p1 = new Products { ProductID = 1, Name = "P1", Price = 100m };
            Products p2 = new Products { ProductID = 2, Name = "P2", Price = 50m };

            Cart target = new Cart();

            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.AddItem(p1, 3); 

            decimal result = target.ComputeTotalValue();

            Assert.Equal(450m, result); 
        }

      

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
        
       

    }
}
