using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models
{
    // класс по­лучения от пользователя подробной информации о доставке.
    public class Order
    {
        [BindNever]
        public int OrderID { get; set; }

        [BindNever]
        public ICollection<CartLine> Lines { get; set; }

        #region BindNeverAttribute
        //Указывает, что свойство должно быть исключено из привязки модели.
        //При применении к свойству система привязки модели исключает это свойство.
        //При применении к типу система привязки модели исключает все свойства, определяемые этим типом.
        #endregion 
        [BindNever] // Инициализирует новый экземпляр BindNeverAttribute. 
        public bool Shipped { get; set; } // для фиксирования заказов - какие были отгружены (по умолчанию bool = false)

        [Required(ErrorMessage = "Please enter the first address line")]
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Please enter a name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a city name")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter a state name")]
        public string State { get; set; }
        public string Zip { get; set; }

        [Required(ErrorMessage = "Please enter a country name")]
        public string Country { get; set; }
        public string GiftWrap { get; set; }

        // Здесь применяются атрибуты проверки достоверности из пространства имен
        // Systern.CornponentModel.DataAnnotations, как мы делали в главе 2. Проверка до­
        // стоверности подробно рассматривается в главе 27. 
        // Кроме того, используется атрибут BindNever, который предотвращает предо­
        // ставление пользователем значений для снабженных им свойств в НТТР-запросе.Это
        // средство системы привязки моделей, которая описана в главе 26
    }
}
