using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models.ViewModels
{
    //Представлению, которое будет отображать содержимое корзины, необходимо пе­
    //редать две порции информации: объект Cart и URL для отображения в случае, если
    //пользователь щелкнет на кнопке Continue shopping(Продолжить покупку). Для этой
    //цели мы создадим простой класс CartIndexViewModel модели представления.

    // модель представления, для метода действия Index()
    public class CartIndexViewModel
    {
        public Cart Сart { get; set; }
        public string ReturnUrl { get; set; }
    }
}
