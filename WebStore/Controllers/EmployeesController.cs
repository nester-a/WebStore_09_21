using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Models;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

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

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                SecondName = employee.Patronymic,
                Age = employee.Age,
            };

            return View(model);
        }
        public IActionResult Create() => View("Edit", new EmployeeViewModel());

        #region Edit

        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeeViewModel());

            var employee = _employeesData.GetById((int)id);
            if (employee is null) return NotFound();

            var model = new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                SecondName = employee.Patronymic,
                Age = employee.Age,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeViewModel model)
        {
            if(model.LastName == "Асама" && model.Name == "Бин" && model.SecondName == "Ладан")
            {
                ModelState.AddModelError("", "Террористов не берём!");
            }
            if (!ModelState.IsValid) return View(model);

            var employee = new Employee
            {
                Id = model.Id,
                FirstName = model.Name,
                LastName = model.LastName,
                Patronymic = model.SecondName,
                Age = model.Age,
            };

            if (employee.Id == 0) 
                _employeesData.Add(employee);
            else
                _employeesData.Update(employee);

            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Delete

        public IActionResult Delete(int id)
        {
            if (id < 0) return BadRequest();

            var employee = _employeesData.GetById(id);
            if (employee is null) return NotFound();

            return View(new EmployeeViewModel
            {
                Id = employee.Id,
                Name = employee.FirstName,
                LastName = employee.LastName,
                SecondName = employee.Patronymic,
                Age = employee.Age,
            });
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _employeesData.Delete(id);
            return RedirectToAction(nameof(Index));
        } 

        #endregion
    }
}
