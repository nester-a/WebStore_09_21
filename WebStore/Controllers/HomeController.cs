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
        private static readonly List<Employee> _employees = new()
        {
            new Employee() { Id = 1, LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Age = 27 },
            new Employee() { Id = 2, LastName = "Петров", FirstName = "Пётр", Patronymic = "Петрович", Age = 31 },
            new Employee() { Id = 3, LastName = "Сидоров", FirstName = "Сидор", Patronymic = "Сидорович", Age = 18 },
        };

        /// <summary>
        /// Действие данного контроллера будет доступно, если мы перейдёи по ссылке
        ///  http://localhost:5000/Home/Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            //return Content("Hello from first controller");

            //если у нас предаставление называется также как и действие
            //то в парамерт метода ничего передавать не нужно
            return View();
        }
        public IActionResult SecondAction(string id)
        {
            return Content($"Second action with parameter {id}");
        }

        public IActionResult Employees()
        {
            return View(_employees);
        }
    }
}
