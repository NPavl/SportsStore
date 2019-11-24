using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    #region IProductRepository
    //Этот инте рфейс использует I E n urneraЫe<T> , чтобы позволить вызывающе­
    //му коду получать последовательность объектов Product , ничего не сообщая о том,
    //как или где хранятся либо извлекаются данные.Класс, зависящий от интерфейса
    //IProductRepos i tory, может получать объекты Product, ничего не зная о том, от­
    //куда они поступают или каким образом класс реализации будет их доставлять. В про­
    //цессе разработки мы еще будем возвращаться к интерфейсу IProduc tRepositor y,
    //чтобы добавлять в него нужные средства . 
    #endregion
    public interface IProductRepository
    {
        IEnumerable<Products> Products { get; }

        #region SaveProduct
        // Прежде чем можно будет обрабатывать результаты редактирования котроллера Edit 
        // хранили­ще товаров понадобится расширить, добавив возможность сохранения изменений.
        // для этого добавим к интерфейсу IProductRepository новый ме­тод
        #endregion

        void SaveProduct(Products product);

        Products DeleteProducts(int productID);

    }
}
