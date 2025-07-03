using EmployeeAccessControlAPI_SOLID.Data;
using EmployeeAccessControlAPI_SOLID.Models;
using EmployeeAccessControlAPI_SOLID.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAccessControlAPI_SOLID.Services
{
    public class ShiftService : IShiftService
    {
        private readonly AppDbContext _context;
        public ShiftService(AppDbContext context)
        {
            _context = context;
        }
        public string StartShift(int employeeId, DateTime startTime)
        {
            var emp = _context.Employees.Include(e => e.Shifts).FirstOrDefault(e => e.Id == employeeId);
            if (emp == null)
                return "Сотрудник не найден.";

            // Правило: если предыдущая смена не завершена, нельзя начать новую.
            if (emp.Shifts.Any(s => s.StartTime != null && s.EndTime == null))
                return "Ошибка: предыдущая смена не завершена.";

            var newShift = new Shift
            {
                EmployeeId = employeeId,
                StartTime = startTime
            };

            _context.Shifts.Add(newShift);
            _context.SaveChanges();
            return "Смена начата.";
        }

        public string EndShift(int employeeId, DateTime endTime)
        {
            var emp = _context.Employees.Include(e => e.Shifts).FirstOrDefault(e => e.Id == employeeId);
            if (emp == null)
                return "Сотрудник не найден.";

            // Правило: если сотрудник не пробил вход, нельзя отметить выход.
            var shift = emp.Shifts.FirstOrDefault(s => s.StartTime != null && s.EndTime == null);
            if (shift == null)
                return "Ошибка: нет открытой смены.";

            if (endTime < shift.StartTime)
                return "Ошибка: время выхода раньше времени входа.";

            if (shift.StartTime.HasValue)
            {
                shift.EndTime = endTime;
                shift.HoursWorked = (endTime - shift.StartTime.Value).TotalHours;
                _context.SaveChanges();
                return "Смена завершена.";
            }
            else
            {
                return "Ошибка: время входа отсутствует.";
            }
        }
    } 
}
