using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Действие данного контроллера будет доступно, если мы перейдёи по ссылке
        ///  http://localhost:5000/Home/Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index() => View();
        public IActionResult Blog() => View();
    }
}
