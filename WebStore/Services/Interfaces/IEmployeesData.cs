using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Domain.Entities;
using WebStore.Models;

namespace WebStore.Services.Interfaces
{
    //здесь интерфейс это то, как можно будет использовать
    //наш сервис снаружи/извне
    //для интерфейса сервиса пишуться исключительно методы (без свойств, событий и прочего)
    public interface IEmployeesData
    {
        IEnumerable<Employee> GetAll();

        Employee GetById(int id);

        int Add(Employee employee);

        void Update(Employee employee);

        bool Delete(int id);
    }
}
