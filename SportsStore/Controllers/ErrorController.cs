using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore.Controllers
{
    // приложение сконфигурировано на использование страниц оши­
    // бок , дружественных к разработчику , которые предоставляют полезную информацию, ког­
    // да случается проблема. Конечные пользователи не должны видеть такую информацию, поэтому
    public class ErrorController : Controller
    {
        public ViewResult Error() => View();// стандартное представление 
    }
}
