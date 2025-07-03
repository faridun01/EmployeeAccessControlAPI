using EmployeeAccessControlAPI_SOLID.Data;
using EmployeeAccessControlAPI_SOLID.Models;
using EmployeeAccessControlAPI_SOLID.Services.Interfaces;
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
                throw new ArgumentNullException(nameof(employee), "Сотрудник не может быть пустым");

            bool exists = _context.Employees.Any(e =>
                e.FirstName == employee.FirstName &&
                e.LastName == employee.LastName &&
                e.MiddleName == employee.MiddleName &&
                e.Position == employee.Position);

            if (exists)
                throw new InvalidOperationException("Сотрудник с такими данными уже существует");

            _context.Employees.Add(employee);
            _context.SaveChanges();
            return employee;
        }

        public void DeleteEmployee(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Недопустимый ID сотрудника", nameof(id));

            var employee = _context.Employees.Find(id);
            if (employee == null)
                throw new KeyNotFoundException($"Сотрудник с ID {id} не найден");

            _context.Employees.Remove(employee);
            _context.SaveChanges();
        }

        public Employee GetEmployee(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Недопустимый ID сотрудника", nameof(id));

            var employee = _context.Employees
                .Include(e => e.Shifts)
                .FirstOrDefault(e => e.Id == id);

            if (employee == null)
                throw new KeyNotFoundException($"Сотрудник с ID {id} не найден");

            return employee;
        }

        public Employee UpdateEmployee(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee), "Сотрудник не может быть пустым");

            var existingEmployee = _context.Employees.Find(employee.Id);
            if (existingEmployee == null)
                throw new KeyNotFoundException($"Сотрудник с ID {employee.Id} не найден");

            existingEmployee.FirstName = employee.FirstName;
            existingEmployee.LastName = employee.LastName;
            existingEmployee.MiddleName = employee.MiddleName;
            existingEmployee.Position = employee.Position;

            _context.SaveChanges();
            return existingEmployee;
        }

        public void GenerateShiftsForCurrentMonth()
        {
            _context.Shifts.RemoveRange(_context.Shifts);
            _context.SaveChanges();

            var employees = _context.Employees.ToList();
            DateTime today = DateTime.Today;
            DateTime startDate = new(today.Year, today.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            Random random = new();

            foreach (var emp in employees)
            {
                DateTime current = startDate;
                while (current <= endDate)
                {
                    if (current.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday)
                    {
                        current = current.AddDays(1);
                        continue;
                    }

                    DateTime startTime = current.Date.AddHours(9);
                    DateTime endTime = emp.Position == Position.CandleTester
                        ? current.Date.AddHours(21)
                        : current.Date.AddHours(18);

                    bool simulateViolation = random.NextDouble() < 0.03;

                    if (simulateViolation)
                    {
                        if (emp.Position == Position.CandleTester)
                            endTime = endTime.AddMinutes(-random.Next(15, 60));
                        else if (random.Next(2) == 0)
                            startTime = startTime.AddMinutes(random.Next(10, 60));
                        else
                            endTime = endTime.AddMinutes(-random.Next(15, 45));
                    }

                    _context.Shifts.Add(new Shift
                    {
                        EmployeeId = emp.Id,
                        StartTime = startTime,
                        EndTime = endTime,
                        HoursWorked = (endTime - startTime).TotalHours
                    });

                    current = current.AddDays(1);
                }
            }

            _context.SaveChanges();
        }

        public IEnumerable<object> GetEmployeesWithViolationCounts()
        {
            var violationDict = GetViolationCountsForAllEmployees();
            var employees = _context.Employees.ToList();

            var result = employees.Select(e => new
            {
                e.Id,
                e.FirstName,
                e.LastName,
                e.MiddleName,
                e.Position,
                ViolationCount = violationDict.ContainsKey(e.Id) ? violationDict[e.Id] : 0
            });

            return result;
        }

        public Dictionary<int, int> GetViolationCountsForAllEmployees()
        {
            var result = new Dictionary<int, int>();
            var employees = _context.Employees.ToList();

            foreach (var employee in employees)
            {
                double expectedHours = employee.Position == Position.CandleTester ? 12 : 9;

                int violationCount = _context.Shifts.Count(s =>
                    s.EmployeeId == employee.Id &&
                    s.HoursWorked < expectedHours);

                result.Add(employee.Id, violationCount);
            }

            return result;
        }

        public int GetViolationCountForEmployee()
        {
            // Example: return total number of violations across all employees
            return _context.Shifts
                .Where(s =>
                    (s.Employee.Position != Position.CandleTester && s.HoursWorked < 9) ||
                    (s.Employee.Position == Position.CandleTester && s.HoursWorked < 12))
                .Count();
        }

    }
}
