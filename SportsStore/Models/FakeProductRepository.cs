using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    // фиктивное хранилище пока не будет создана реальная БД 
    public class FakeProductRepository // :  IProductRepository // реализует интерфейс IProductRepository 
                                       // в последствии мы закоментировали интерфейс IProductRepository так как 
                                       // Имитированное хранилище использовалось для быстрого старта процес­
                                       // са разработки и демонстрации возможности применения служб для гладкой заме­
                                       // ны реализаций интерфейса, не изменяя компоненты, которые на них опираются.
                                       // Имитированное хранилище больше не понадобится.
    {
        public IEnumerable<Products> Products => new List<Products> // свойство которое возвращает лист закрытое типом  Products
        {
        new Products {Name = "Football", Price= 25},
        new Products {Name = "Surf board", Price= 179},
        new Products {Name = "Running shoes", Price= 95},
        };

    }
}
