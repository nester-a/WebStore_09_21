using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEnumerable<Employee> _employees;
        public EmployeesController()
        {
            _employees = TestData.Employees;
        }
        public IActionResult Index() => View(_employees);
        public IActionResult Details(int id)
        {
            var employee = _employees.SingleOrDefault(e => e.Id == id);
            if (employee is null) 
                return NotFound();

            return View(employee);
        }
    }
}
