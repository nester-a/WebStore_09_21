using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    //маршрут по которому будет доступен контроллер
    [Route("Staff/[action]/{id?}")]
    public class EmployeesController : Controller
    {
        private readonly IEnumerable<Employee> _employees;
        public EmployeesController()
        {
            _employees = TestData.Employees;
        }

        //маршрут, по которому будет доступно действие контроллера
        //[Route("~/employees/all")]
        public IActionResult Index() => View(_employees);

        //маршрут, по которому будет доступно действие контроллера
        //[Route("~/employees/info-{id}")]
        public IActionResult Details(int id)
        {
            var employee = _employees.SingleOrDefault(e => e.Id == id);
            if (employee is null) 
                return NotFound();

            return View(employee);
        }
    }
}
