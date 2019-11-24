using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// Определение начальных данных Мы планируем явно создать пользователя Admin, наполняя базу данных началь­
// ными данными при запуске приложения.
namespace SportsStore.Models
{
    public static class IdentitySeedData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";

        #region  EnsurePopulated - UserManager
        // В коде применяется класс UserManager<T>, который предоставляется системой
        // ASP.NEТ Core Identity в виде службы для управления пользователями, как описано в
        // главе 28. В базе данных производится поиск учетной записи пользователя Admin, ко­
        // торая в случае ее отсутствия создается(с паролем Secret123$).Не изменяйте жестко
        // за кодированный пароль в этом примере, поскольку система Identity имеет политику
        // проверки достоверности, которая требует, чтобы пароли содержали цифры и диапа­
        // зон символов. Способ изменения настроек, относящихся к проверке достоверности,
        // описан в главе 28.

        // Внимание! Жесткое кодирование деталей учетной записи администратора часто требуется
        // для того, чтобы можно было войти в приложе ние после его развертывания и начать ад­
        // министрирование.Поступая так, вы должны помнить о необходимости изменения пароля
        // для учетной записи , которую создали . В главе 28 приведены детали того, как изменять
        // пароли , используя ldeпtity. 
        #endregion
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            UserManager<IdentityUser> userManager = app.ApplicationServices
                .GetRequiredService<UserManager<IdentityUser>>();

            IdentityUser user = await userManager.FindByIdAsync(adminUser);
            if (user == null)
            {
                user = new IdentityUser("Admin");
                await userManager.CreateAsync(user, adminPassword);
            }

        }


    }
}
