using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Models.ViewModels
{
    #region описание класса
    // Когда пользователь. не прошедший аутентификацию, посылает запрос, который
    // требует авторизации, он будет перенаправлятся на URL вида /Account/Login, где прило­
    // жение может пригласить пользователя ввести свои учетные данные. В качестве под­
    // готовки создаем модель представления для учетных данных пользователя, добавив в
    // класс LoginModel.cs.

    // Свойства Name и Password декорированы атрибутом Required, который исполь­
    // зует проверку достоверности модели для обеспечения того, что значения были пре­
    // доставлены. Свойство Password декорировано атрибутом UIHint, поэтому в случае
    // применения атрибута asp-for внутри элемента input представления Razor, пред­
    // назначенного для входа, дескрипторный вспомогательный класс установит атрибут
    // type в password; таким образом, вводимый пользователем текст не будет виден на
    // экране. Использование атрибута UIHint описано в главе 24. 
    #endregion

    public class LoginModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [UIHint("password")] 
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
