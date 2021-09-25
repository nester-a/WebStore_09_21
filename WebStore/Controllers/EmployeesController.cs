using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
    //маршрут по которому будет доступен контроллер
    [Route("Staff/[action]/{id?}")]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _employeesData;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(IEmployeesData employeesData, ILogger<EmployeesController> logger)
        {
            _employeesData = employeesData;
            _logger = logger;
        }

        //маршрут, по которому будет доступно действие контроллера
        //[Route("~/employees/all")]
        public IActionResult Index() => View(_employeesData.GetAll());

        //маршрут, по которому будет доступно действие контроллера
        //[Route("~/employees/info-{id}")]
        public IActionResult Details(int id)
        {
            var employee = _employeesData.GetById(id);
            if (employee is null) 
                return NotFound();

            return View(employee);
        }
    }
}
