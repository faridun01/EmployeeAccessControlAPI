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
            var emp = _context.Employees
                .Include(e => e.Shifts)
                .FirstOrDefault(e => e.Id == employeeId);

            if (emp == null)
                return "Сотрудник не найден.";

            // Проверка: начало смены не может быть позже 10:00
            if (startTime.TimeOfDay > new TimeSpan(12, 0, 0))
                return "Ошибка: смена не может начаться позже 12:00 am.";

            // Проверка: если есть незакрытая смена — ошибка
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
            var emp = _context.Employees
                .Include(e => e.Shifts)
                .FirstOrDefault(e => e.Id == employeeId);

            if (emp == null)
                return "Сотрудник не найден.";

            // Найти открытую смену
            var shift = emp.Shifts.FirstOrDefault(s => s.StartTime != null && s.EndTime == null);
            if (shift == null)
                return "Ошибка: нет открытой смены.";

            if (!shift.StartTime.HasValue)
                return "Ошибка: время входа отсутствует.";

            // Проверка: окончание смены должно быть в тот же день, что и начало
            if (endTime.Date != shift.StartTime.Value.Date)
                return "Ошибка: нельзя завершить смену в другой день, отличный от начала.";

            // Проверка: выход не может быть раньше входа
            if (endTime < shift.StartTime)
                return "Ошибка: время выхода раньше времени входа.";

            // Завершение смены
            shift.EndTime = endTime;
            shift.HoursWorked = (endTime - shift.StartTime.Value).TotalHours;

            _context.SaveChanges();
            return "Смена завершена.";
        }


    }
}
