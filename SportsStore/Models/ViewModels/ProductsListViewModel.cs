using System;
using System.Collections.Generic;
using System.Linq;  
using System.Threading.Tasks;

namespace SportsStore.Models.ViewModels
{   // класс для снабжения представления сведениями о товарах, отображаемых на страницах.
    // и информацией о разбиении на страницы

    public class ProductsListViewModel
    {
        public IEnumerable<Products> Products { get; set; }
        public PagingInfo PagingInfo { get; set; } // класс модели представления ( как доп информация)
        //свойство для указания категории
        public string CurrentCategory { get; set; } // свойство для взаимодействия текущей категории 
                                                    // с представлением чтобы визуализировать боковую панель 
    }
}
