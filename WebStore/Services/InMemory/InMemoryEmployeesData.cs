using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Domain.Entities;
using WebStore.Models;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InMemory
{
    public class InMemoryEmployeesData : IEmployeesData
    {
        private readonly ILogger<InMemoryEmployeesData> _logger;
        private int _currentMaxId;
        public InMemoryEmployeesData(ILogger<InMemoryEmployeesData> logger)
        {
            _logger = logger;
            _currentMaxId = TestData.Employees.Max(e => e.Id);
        }
        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            if (TestData.Employees.Contains(employee)) return employee.Id;

            employee.Id = ++_currentMaxId;
            TestData.Employees.Add(employee);
            return employee.Id;
        }

        public bool Delete(int id)
        {
            var db_employee = GetById(id);
            if (db_employee is null) return false;

            TestData.Employees.Remove(db_employee);
            return true;
        }

        public IEnumerable<Employee> GetAll()
        {
            return TestData.Employees;
        }

        public Employee GetById(int id)
        {
            return TestData.Employees.SingleOrDefault(e => e.Id == id);
        }

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            //эта строка нужна только для реализации сервиса "в памяти"
            if (TestData.Employees.Contains(employee)) return;

            var db_employee = GetById(employee.Id);
            if (db_employee is null) return;

            db_employee.FirstName = employee.FirstName;
            db_employee.LastName = employee.LastName;
            db_employee.Patronymic = employee.Patronymic;
            db_employee.Age = employee.Age;

            //db.SaveChanges();
        }
    }
}
