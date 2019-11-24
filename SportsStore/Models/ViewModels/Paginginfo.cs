using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models.ViewModels
{
    #region Добавление модели представления 
    //Чтобы обеспечить поддержку дескрипторного вспомогательного класса 
    //PageLinkTagHelper, мы соби­
    //раемся передавать представлению информацию о количестве доступных страниц, 
    //текущей странице и общем числе товаров в хранилище. Проще всего это сделать,
    //создав класс модели представления, который специально примен яется для переда­
    //чи данных между контроллером и представлением.
    #endregion
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
        
        // метод Ceiling Возвращает наименьшее целое число, которое больше или равно указанному числу.

    }
}
