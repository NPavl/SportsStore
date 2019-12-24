using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    public interface IProductRepository
    {
        IEnumerable<Products> Products { get; }
       
        void SaveProduct(Products product);

        Products DeleteProducts(int productID);

    }
}
