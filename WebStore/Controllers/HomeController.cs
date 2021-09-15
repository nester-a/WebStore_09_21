using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Действие данного контроллера будет доступно, если мы перейдёи по ссылке
        ///  http://localhost:5000/Home/Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return Content("Hello from first controller");
        }
        public IActionResult SecondAction(string id)
        {
            return Content($"Second action with parameter {id}");
        }
    }
}
