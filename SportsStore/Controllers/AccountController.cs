using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    #region AccountController
    // Когда пользователь перенаправляется на URL вида /Account/Login, версия GET
    // метода действия Login() визуализирует стандартное представление для страницы и
    // создает объект модели представления, включающий URL, на который браузер должен
    // быть перенаправлен, если запрос на аутентификацию завершился успешно.
    // Учетные данные аутентификации отправляются версии POST метода дейс­
    // твия Login (), которая применяет службы Us e r Manager<Identi tyUser> и
    // SigninManager<Identi tyUser>, полученные через конструктор класса контрол­
    // лера, для аутентификации пользователя и его входа в систему. Работа упомянутых
    // классов объясняется в главах 28-30, а пока достаточно знать, что в случае отказа в
    // аутентификации создается ошибка проверки достоверности модели и визуализиру­
    // ется стандартное представление. Если же аутентификация прошла успешно, тогда
    // пользователь перенаправляется на URL, к которому он хотел получить доступ перед
    // тем, как ему было предложено ввести свои учетные данные. 

    // Внимание! В целом использование проверки достоверности данных на стороне клиента яв­
    // ляется хорошей практикой.Она освобождает от определенной работы сервер и обеспе­
    // чивает пользователям немедленный отклик о предоставленных ими данных . Тем не менее,
    // не поддавайтесь искушению выполнять на стороне клиента аутентификацию, поскольку
    // это обычно предусматривает передачу клиенту допустимых учетных данных, которые бу­
    // дут применяться при проверке вводимых имени пользователя и пароля, или , по меньшей
    // мере, наличие доверия сообщению клиента о том, что аутентификация завершилась ус­
    // пешно. Аутентификация должна всегда выполняться на сервере . 
    #endregion
    [Authorize]
    public class AccountController : Controller
    {
        #region класс UserManager IdentityUser SignInManager
        // UserManager - Предоставляет API для управления пользователем в постоянном хранилище.
        // IdentityUser - готовый класс коорый описывает , много способов аутенификации 
        // SignInManager Предоставляет API для входа пользователя.
        #endregion
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager; // класс SignInManager - Provides the APIs for user sign in.

        public AccountController(UserManager<IdentityUser> userMgr, SignInManager<IdentityUser> signInMgr)
        {
            userManager = userMgr;
            signInManager = signInMgr;
        }

        [AllowAnonymous] // атрибут Указывает, что классу или методу, к которому применяется этот атрибут, нетребуется авторизация.
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        #region ValidateAntiForgeryToken
        // Указывает, что класс или метод, к которому применяется этот атрибут, проверяет
        // токен против подделки. Если токен подделки недоступен или токен
        // неверен, проверка не пройдена и метод действия не будет выполнен.
        // Примечания.
        // Этот атрибут помогает защитить от подделки межсайтовых запросов. Это не помешает
        // другие подделки или фальсификации атак.
        #endregion
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            #region ModelState
            // Кроме валидации на стороне клиента, мы можем осуществлять валидацию и на сервере внутри контроллера.
            // Делается это с помощью проверки значения свойства ModelState.IsValid.
            // Объект ModelState сохраняет все значения, которые пользователь ввел для свойств модели, 
            // а также все ошибки, связанные с каждым свойством и с моделью в целом. Если в объекте ModelState 
            // имеются какие-нибудь ошибки, то свойство ModelState.IsValid возвратит false
            #endregion
            if (ModelState.IsValid)
            {
                IdentityUser user = await userManager.FindByNameAsync(loginModel.Name);
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                    {
                        return Redirect(loginModel?.ReturnUrl ?? "/Admin/Index");
                    }
                }
            }

            ModelState.AddModelError(" ", "Invalid name or password");
            return View(loginModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }
    }
}
