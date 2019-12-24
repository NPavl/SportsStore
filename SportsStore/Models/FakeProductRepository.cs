using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    
    public class FakeProductRepository 
    {
        public IEnumerable<Products> Products => new List<Products> 
        {
        new Products {Name = "Football", Price= 25},
        new Products {Name = "Surf board", Price= 179},
        new Products {Name = "Running shoes", Price= 95},
        };

    }
}
