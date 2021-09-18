using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
    }
}
