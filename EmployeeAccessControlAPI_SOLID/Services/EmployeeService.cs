using EmployeeAccessControlAPI_SOLID.Data;
using EmployeeAccessControlAPI_SOLID.Models;
using EmployeeAccessControlAPI_SOLID.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAccessControlAPI_SOLID.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public Employee AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Сотрудник не может быть пустым");
            }
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public void DeleteEmployee(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Недопустимый ID сотрудника", nameof(id));
            }
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Сотрудник с ID {id} не найден");
            }
            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }

        public Employee GetEmployee(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Недопустимый ID сотрудника", nameof(id));
            }
            var employee = _context.Employees.Find(id);
            if (employee == null)
            {
                throw new KeyNotFoundException($"Сотрудник с ID {id} не найден");
            }
            return employee;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee), "Сотрудник не может быть пустым");
            }
            var existingEmployee = _context.Employees.Find(employee.Id);
            if (existingEmployee == null)
            {
                throw new KeyNotFoundException($"Сотрудник с ID {employee.Id} не найден");
            }
            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.MiddleName = employee.MiddleName;
            existingEmployee.Position = employee.Position;
            _context.SaveChanges();
            return existingEmployee;
        }
    }
}
