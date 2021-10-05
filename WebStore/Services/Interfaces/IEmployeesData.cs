using System.Collections.Generic;
using WebStore.Domain.Entities;

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
