using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL
{
    public class SqlEmployeesData : IEmployeesData
    {
        private int _currentMaxId;
        private readonly WebStoreDB _db;
        public SqlEmployeesData(WebStoreDB db)
        {
            _db = db;
            _currentMaxId = _db.Employees.Max(e => e.Id);
        }

        public int Add(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            if (_db.Employees.Contains(employee)) return employee.Id;

            employee.Id = ++_currentMaxId;
            _db.Employees.Add(employee);
            _db.SaveChanges();
            return employee.Id;
        }

        public bool Delete(int id)
        {
            var db_employee = GetById(id);
            if (db_employee is null) return false;

            _db.Employees.Remove(db_employee);
            _db.SaveChanges();
            return true;
        }

        public IEnumerable<Employee> GetAll() => _db.Employees;

        public Employee GetById(int id) =>_db.Employees.SingleOrDefault(e => e.Id == id);

        public void Update(Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            if (_db.Employees.Contains(employee)) return;

            var db_employee = GetById(employee.Id);
            if (db_employee is null) return;

            db_employee.FirstName = employee.FirstName;
            db_employee.LastName = employee.LastName;
            db_employee.Patronymic = employee.Patronymic;
            db_employee.Age = employee.Age;

            _db.SaveChanges();
        }
    }
}
