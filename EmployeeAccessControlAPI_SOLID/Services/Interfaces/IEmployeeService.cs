using EmployeeAccessControlAPI_SOLID.Models;

namespace EmployeeAccessControlAPI_SOLID.Services.Interfaces
{
    public interface IEmployeeService
    {
        Employee AddEmployee(Employee employee);
        Employee GetEmployee(int id);
        Employee UpdateEmployee(Employee employee);
        void DeleteEmployee(int id);
    }
}
