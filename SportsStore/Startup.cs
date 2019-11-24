using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SportsStore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

//конфигурационный файл Startup описание его основных методов ConfigureServices и Configure
//есть на сайте метанит https://metanit.com/sharp/aspnet5/2.1.php
//а также в папке проектов Razor, Razor_1, WorkingWithVisualStudio по ходу книги есть описание

namespace SportsStore
{
    #region Класс Startup
    //Класс Startup является входной точкой в приложение ASP.NET Core.Этот класс производит 
    //конфигурацию приложения, настраивает сервисы, которые приложение будет использовать, 
    //устанавливает компоненты для обработки запроса или middleware.
    #endregion
    public class Startup
    {
        IConfigurationRoot Configuration; // IConfigurationRoot - Представляет корень иерархии IConfiguration.
        // IConfiguration - Представляет набор свойств конфигурации приложения в вид пар "ключ — значение".
        public Startup(IHostingEnvironment env) // IHostingEnvironment - Предоставляет информацию о среде веб-хостинга, 
                                                // в которой запущено приложение. Этот тип устарел и будет удален в следующей версии. Рекомендуемая альтернатива - Microsoft.AspNetCore.Hosting.IWebHostEnvironment.
        {
            #region Configuration
            //Добавленный в класс Startup конструктор загружает конфигурационные на­
            //стройки из файла appsettings.js on и делает их доступными через свойство по
            //имени Configuration. 
            #endregion
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .Build();
            // дпоплнительной строкой (.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true).Build();) ниже подключаем БД Azure 
        }

        #region ConfigureServices
        // Необязательный метод ConfigureServices() регистрирует сервисы, которые используются приложением.
        // В качестве параметра он принимает объект IServiceCollection, который и представляет коллекцию сервисов 
        // в приложении.С помощью методов расширений этого объекта производится конфигурация приложения для 
        // использования сервисов. Все методы имеют форму Add[название_сервиса].

        //Метод ConfigureServices применяется для настройки разделяемых объектов, 
        //которые могут использоваться повсеместно в приложении через средство внедрения
        //зависимостей, которое рассматривается в главе 18. Метод AddMvc(), вызываемый
        //внутри метода ConfigureServices(), является расширяющим методом, который
        //настраивает разделяемые объекты, применяемые в приложении MVC.
        #endregion
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddTransient<IProductRepository, EFProductRepository>(); 
            #region  AddDbContext. настраивает инфраструктуру Entity Framework Core
            //добавлена последовательность обращений к мето­дам, которая настраивает инфраструктуру Entity Framework Core.

            //Расширяющий метод AddDbContext() настраивает службы, предоставляемые
            //инфраструктурой Entity Framework Core для класса контекста базы данных.кото­
            //рый был создан в листинге 8.17.Как объяснялось в главе 14, многие методы, ис­
            //пользуемые в классе Startup, позволяют конфигурировать службы и средства про­
            //межуточного программного обеспечения с применением аргументов с параметр ами . 
            //Аргументом метода AddDbContext() является лямбда-выражение , которое получает
            //объект options, конфигурирующий базу данных для класса контекста.В этом случае
            //база данных конфигурируется с помощью метода UseSqlServer(}
            //и указания стро­
            //ки подключения, которая получена из свойства Configuration.

            //Еще одно изменение, внесенное в класс Startup, было связано с заменой фиктив­
            //ного хранилища реальным EFProductRepository : services.AddTransient<IProductRepository, EFProductRepository>();

            //Компоненты в приложении, использующие интерфейс IProductRepository, к
            //которым в настоящий момент относится только контроллер Product, при создании
            //будут получать объект EFProductReposi tory, предоставляющий им доступ к инфор­
            //мации в базу данных.Подробные объяснения будут даны в главе 18, а пока просто
            //знайте, что результатом будет гладкая замена фиктивных данных реальными из базы
            //данных без необходимости в изменении класса ProductController. 
            #endregion
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:SportStoreProducts:ConnectionString"]));

            #region Конфигурирование средства ldeпtity подробно механика подкюлчения есть в уроке Эдуко миграции на 1.51.00
            // Конфигурирование средства ldeпtity
            //конфигурация Entity Framework Core расширена для регистрации класса контекста с помощью метода Addidentity() устанавли­вает 
            // службы Identity, используя встроенные классы для представления пользователей и ролей.
            // Урок Эдуко на 1.51.00 про миграции данных там подробно про подключение Identity и насройку класса Sturtup при подключении эих серввисов
            #endregion 
            #region IdentityUser
            //готовый класс коорый описывает , много способов аутенификации 
            #endregion
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["Data:SportStoreIdentity:ConnectionString"]));
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
            #region Dependency Injection (AddTransient AddScoped AddSingleton) различия 
            // AddTransient обьединяет интерфейс и класс который реализует этот интерфейс 
            // пример того как работает DI к примеру ку нас есть EFProductRepository который реализует IProductRepository 
            // и мы можем покючить его тремя способами AddTransient AddScoped AddSingleton , 
            // AddTransient - это на каждый запрос будет создаватся новый экземпляр данного класса EFProductRepository
            // AddScoped - при реализации сервиса IProductRepository буде создавася только один раз на запрос (удобно для коннекшенов в базе
            // чтобы не плодить их множество при обработке одного запроса)
            // и AddSingleton - это когда создаетс один экземплр на ваше приложение , тоесть любой реквест на IProductRepository будет реализован 
            // DI механизмом в один и тотже обьект который мы указали  EFProductRepository
            #endregion
            services.AddTransient<IProductRepository, EFProductRepository>();
            ////------------------------------
            #region Создание службы корзины (для класса Cart) AddScoped AddSingleton
            // Создание службы корзины (для класса Cart)  Цель в том, чтобы удовлетворять запросы для 
            // объектов Cart выдачей объектов SessionCart, которые будут сохранять себя самостоятельно.

            // Метод AddScoped() уиазывает, что для удовлетворения связанных запросов к эк­
            // земплярам Cart должен применяться один и тот же объект. Способ связывания за­
            // просов может быть сконфигурирован, но по умолчанию это значит, что в ответ на
            // любой запрос энземпляра Cart со стороны компонентов , ноторые обрабатывают тот
            // же самый НТГР-запрос, будет выдаваться один и тот же объект.
            // Вместо предоставления методу AddScope d() отображения между типами, как
            // делалось для хранилища, уназывается лямбда - выражение, которое будет вьmолнять­
            // ся для удовлетворения запросов к Cart.Лямбда - выражение получает коллекцию
            // служб, которые были зарегистрированы, и передает ее методу GetCart() класса
            // SessionCart.В результате запросы для службы Ca r t будут обрабатываться путем
            // создания объектов Sess i onCart, которые сериали зируют сами себя как данные се­
            // анса, когда они модифицируются . 
            // Мы также добавили службу с использованием метода AddSingleton() ,который
            // указывает, что всегда должен применяться один и тот же объект. Созданная служба
            // сообщает инфраструктуре MVC о том, что когда требуются реализации интерфейса
            // IHttpContextAccessor, необходимо использовать класс HttpContextAccessor.
            // Данная служба обязательна, поэтому в классе SessionCart можно получать доступ
            // к текущему сеансу, как делалось в листинге 1О. 1.
            #endregion
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IOrderRepository, EFOrderRepository>(); // регистрация хранилища заказов в службе

            //------------------------------
            #region AddTransient FakeProductRepository
            // services.AddTransient<IProductRepository, FakeProductRepository>();
            // Добавленный в метод ConfigureServices() оператор сообщает инфраструкту­
            //ре ASP.NET о том, что когда компоненту наподобие контроллера необходима реали­
            //зация интерфейса IProductRepository, она должна получить экземпляр класса
            //FakeProductRepository. Метод Add Transient() указывает, что каждый раз.когда
            //требуется реализация интерфейса IProductRepository, должен создаваться новый
            //объект FakeProductRepository.
            //нужно для того чтобы зменять FakeProductRepository на реальный обьект БД невнося
            //изменения во все классы которым нужен доступ к БД
            //---
            //Transient-объекты создаются каждый раз, когда нам требуется экземпляр определенного класса https://metanit.com/sharp/aspnet5/6.5.php
            #endregion
            services.AddMvc(); // использование архитектуры MVC
            services.AddMemoryCache(); // Добавляет нераспределенную в памяти реализацию Microsoft.Extensions.Caching.Memory.IMemoryCache
            services.AddSession(); //Adds services required for application session state. Добавляет службы, необходимые для состояния сеанса приложения.

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        #region Метод Configure()
        //Метод Configure устанавливает, как приложение будет обрабатывать запросы.
        //Этот метод является обязательным. Для установки компонентов, которые обрабатывают запрос, 
        //используются методы объекта IApplicationBuilder. Объект IApplicationBuilder является 
        //обязательным параметром для метода Configure.

        //Кроме того, метод нередко принимает еще один необязательный параметр - объект IWebHostEnvironment, 
        //который позволяет получить информацию о среде, в которой запускается приложение, и взимодействовать с ней.
        // Метод Configure() класса Startup применяется для настройки конвейера за­
        // просов, состоящего из классов(известных как промежуточное программное обеспе­
        // чение), которые будут инспектировать НТTР-запросы и генерировать ответы.
        //----
        //Метод Configure() используется для настройки средств, которые получают и
        //обрабатывают НТГР-запросы. Каждый метод. вызываемый в методе Configure(),
        //представляет собой расширяющий метод, который настраивает средство обработки
        //НТTР-запросов
        #endregion
        #region IHostingEnvironment
        //Интерфейс IHostingEnvironment используется для предоставления информации
        //о среде, в которой функционирует приложение, такой как среда разработки или про­
        //изводственная среда.
        //Мы воспользовались преимуществом этого средства для загрузки разных конфи­
        //гурационных файлов с подходящими строками подключений, ориентированными на
        //ср еду разработки и производств е нную среду.а также изменения набор а ко мпоне нтов.
        //которы е применяются для обработки запросов.Таким обр азом, средства , сп ецифич­
        //ны е для р азработк и, вроде Browser Link не включаются, когда приложение р аз в е рты­
        //вает ся.Доступно множество параметров для настройки конфигурации приложения в
        //р азличных средах.которые будут рассматриваться в главе 14. 
        #endregion
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) // 
        {

            if (env.IsDevelopment())
            {
                #region UseDeveloperExceptionPage
                //Этот расширяющий метод отображает детали исключе­
                //ния , которое произошло в приложении, что полезно во
                //время процесса разработки. Он не должен быть вклю­
                //чен в развернутых приложениях; в главе 12 будет пока­
                //зано , как отключить данное средство
                #endregion
                app.UseDeveloperExceptionPage();
                #region UseStatusCodePages();
                //Этот расширяющий метод добавляет простое сообще ­
                //ние в НТТР - ответы, которые иначе бы не имели тела,
                //  такие как ответы 404 - Not Found(404 - не найдено)
                #endregion
                app.UseStatusCodePages();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            #region UseStaticFiles
            //Этот расширяющий метод включает поддержку для
            //обслуживания статического содержимого из папки
            //wwwroot
            #endregion
            app.UseStaticFiles();
            #region UseMvcWithDefaultRoute
            //Этот расширяющий метод включает инфраструктуру
            //ASP.NET Core MVC со стандартной конфигурацией
            //(которая позже в процессе разработки будет изменена)
            #endregion
            // app.UseMvcWithDefaultRoute(); 
            app.UseSession(); //Добавляет Microsoft.AspNetCore.Session.SessionMiddleware для автоматического включения состояние сеанса для приложения.

            #region UseIdentity()
            // настройку Identity мы произвели в методе ConfigureServices здесь в Services мы ее включаем методом UseIdentity
            // метод UseIdentity() нужен для установки компонентов, которые будут перехватывать запросы и ответы для внедрения 
            //политики безопасности. Кроме того, добавлен вызов метода IdentitySeedData.EnsurePopulated(), ко­торый будет создан в следующем 
            //разделе для добавления данных о пользователях в базу данных.

            //app.UseAuthentication(); - данный метод подключает утентификацию через БД (логин пароль) если нам нужн подключить какие то
            //внешние провайдеры далее app.UseAuthentication().USeFaceBookAuthentication ... и т д  (в данном случа содержеся много готовых решений)
            //нужно будет только настраивать токены которые будет (к пример убудут смотрть наш фейсбук аккаунт , ии гугл т д ) - много готовых 
            //механик подключения не нужно писать вручную нужно толко зарегстировать службу в настройках какой api уде использоватся.
            #endregion
            app.UseIdentity();

            #region UseMvc()
            // Инфраструктуре MVC понадобится сообщить, что она должна отправлять запросы,
            // поступающие для корневого URL приложения(http://мой-сайт/ ), методу действия 
            // List() класса ProductController. 
            // Метод UseMvc() настраивает промежуточное программное обеспечение МVС, причем одним
            // из параметров конфигурации является схема, которая будет использоваться для со­
            // поставления URL с контроллерами и методами действий.

            // указывают инфраструктуре MVC на необходимость
            // отправки запросов методу действия List() контроллера Product, если только в URL
            // запроса не указано иное.

            // имя контроллера указано как Product, а не
            // ProductController, являющееся именем класса. Это часть соглашения об именова­
            // нии MVC, в рамках которого имена классов обычно заканчиваются словом Controller, 
            // но при ссылке на класс данная часть имени опускается.
            #endregion
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "Error", template: "Error", defaults: new { controller = "Error", action = "Error" });

                #region маршрут pagination описание
                //Важно поместить этот маршрут перед стандартным маршрутом (по имени
                //default). который уже присутствует в методе. Как будет показано в главе 15, система
                //маршрутизации обрабатывает маршруты в порядке их перечисления, а нам нужно,
                //чтобы новый маршрут имел преимущество перед существующим. 
                #endregion
                // при запросе в браузере к нашему списку товаров, разбитому на N кол-ва страниц,
                // текущий адрес в браузере будет такой https://localhost:44367/Products/page1  или 
                // такой https://localhost:44367/Product/List/?Page=1  - как второй маршрут по умолчанию - default
                // и один и второй отображет одинаковые данные - контроллера Product action List
                #region старые маршруты
                //routes.MapRoute
                //(
                //    name: "pagination",
                //    template: "Products/Page{page}",
                //    defaults: new { Controller = "Product", action = "List" });

                //// маршрут доступа к контроллеру Product по умолчанию 
                //routes.MapRoute
                //(
                //    name: "default",
                //    template: "{controller=Product}/{action=List}/{id}");
                #endregion
                //---------------------------------------
                routes.MapRoute
               (
                   name: null,
                   template: "{category}/Page{page:int}",
                       defaults: new { controller = "Product", action = "List" }
                       ); // вид ссылки https://localhost:44367/Soccer/page1 - отображает список товаров по категории и текущей страницей

                routes.MapRoute
                    (
                        name: null,
                        template: "Page{page:int}",
               defaults: new { controller = "Product", action = "List", page = 1 }
               ); // вид ссылки https://localhost:44367/page1 - отображает список товаров первая страница

                routes.MapRoute
                    (
                        name: null,
                        template: "{category}",
               defaults: new { controller = "Product", action = "List", page = 1 }
               ); //  вид ссылки https://localhost:44367/Soccer - отображает список товаров по категории 

                routes.MapRoute
                    (
                        name: null,
                        template: "",
               defaults: new { controller = "Product", action = "List", page = 1 }
               ); // вид ссылки https://localhost:44367/ - отображает список товаров не по категории все что есть 

                routes.MapRoute
                    (
                        name: null, template: "{controller}/{action}/{id?}"); // id? вопрос означает что параметр н обязательный
                // https://localhost:44367/?id=1 (отображает 1 страницу со списком id=1 id=3 не работает) 
                // https://localhost:44367/List/?id=1 или https://localhost:44367/List/?id1  https://localhost:44367/Product/?id=1 
                // - пуста страница без списка товаров так как нет List или Product

            });
            #region SeedData.EnsurePopulated метод после всех настроек EF заполняет БД данными (вызов метода перенесен в using ниже)
            //Финальное изменение в классе Startup касается вызова метода SeedData.EnsurePopulated
            //. который гарантирует наличие в базе данных определенной
            //тестовой информации и вызывается внутри метода Configure() класса Startup. 
            //При запуске приложения метод Startup.ConfigureServices() вызывается пе­
            //ред методом Startup.Configure().т.е.ко времени вызова метода  
            //EnsurePopulated() можно иметь уверенность в том, что службы Entity Framework
            //Core уже установлены и сконфигурированы.
            #endregion

            #region using CreateScope (решение проблемы) перенос из класса seeData

            //ApplicationDbContext мы зарегистрировали как сервис с областью действия(тоесть к нему можно обращатся из различных файлов и т д
            //в Startup мы зарегистрировали его как сервис) при этом мы пытались (по книге) добраться к нему вне сервиса(из класса seeData метода EnsurePopulated) 
            // через команду ApplicationDbContext content = app.ApplicationService.GetRequiredService<ApplicationDbContext>(); - ЭТОТ МЕТО НЕ ПРОКАТИЛ 
            // МЫ ПОЛУЧАЛИ ОШИБКУ "Cannot resolve scoped service 'SportsStore.Models.ApplicationDbContext' from root provider." . выход был найден 
            // в видео Гоши Дударя по уроку : 
            // для того чтобы рабоать с ApplicationDbContext не выходя из сервиса мы перенесли его создание в Startup в метод Configure чтобы окружить 
            // его в нужную нам область с указанием метода создания области CreateScope, так как переменная content ограничивается областью 
            // действия оператора using и далее за телом мы немогли с ней работать, теперь в последствии будем передавать 
            //перменную context, как текущий контекст в метод EnsurePopulated все в том же блоке using тд в этой же области дйствия оперетора using. 

            #endregion  
            using (var scope = app.ApplicationServices.CreateScope())
            {
                ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                SeedData.EnsurePopulated(context);
                //который будет создан в следующем разделе для добавления данных о пользователях в базу данных. 
            }
            IdentitySeedData.EnsurePopulated(app); // здесь нужно было содать аналогичный блок using но я пока так и не понял как именно 
                                                   // поэтому воспользовалс ответом из форума и отключил в Program.cs Scope сервис следующй строкой : 
                                                   // .UseDefaultServiceProvider(options => options.ValidateScopes = false);


            //using (var scope = app.ApplicationServices.CreateScope())
            //{
            //    AppIdentityDbContext context = scope.ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            //    IdentitySeedData.EnsurePopulated(app);
            //}

            //using (var scope = app.ApplicationServices.CreateScope())
            //{
            //    //Resolve ASP .NET Core Identity with DI help
            //    var userManager = (UserManager<AppIdentityDbContext>)scope.ServiceProvider.GetService(typeof(UserManager<AppIdentityDbContext>));
            //    // do you things here
            //    IdentitySeedData.EnsurePopulated(app);
            //}




        }
    }
}

