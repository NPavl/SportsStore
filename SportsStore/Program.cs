using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SportsStore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
        .UseDefaultServiceProvider(options => options.ValidateScopes = false);
        #region  решение проблемы проверки областей видимости Scope сервис
        // решение проблемы проверки областей видимости:
        // .UseDefaultServiceProvider(options => options.ValidateScopes = false);
        // отключить проверку областей видимости для сервисов, поставляемых средством DI. 
        // Это делается в Program.cs, найдите место создания IWebHostBuilder
        // ошибку перестало выбивать  
        // https://ru.stackoverflow.com/questions/760698/cannot-resolve-scoped-service-microsoft-aspnetcore-identity-usermanager-from-r
        #endregion


        //UseStartup устанавливает класс Startup в качестве стартового. 
        //И при запуске приложения среда ASP.NET будет искать в сборке приложения 
        //класс с именем Startup и загружать его.
        //Однако в принципе необязательно, что класс назывался именно Startup.
        //Так мы можем изменить соответствующий вызов в файле Program.cs на следующий:

        //.UseStartup<Proccessor>()
        //Теперь среда будет искать при запуске приложения класс Proccessor. 
        //И в этом случае нам надо будет определить в проекте класс с именем Proccessor, 
        //который будет аналогичен файлу Startup.

    }
}
