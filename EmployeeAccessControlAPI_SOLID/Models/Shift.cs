using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessControlAPI_SOLID.Models
{
    public class Shift
    {
        [Key]
        public int Id { get; set; }
        // Время начала смены
        public DateTime? StartTime { get; set; }
        // Время окончания смены
        public DateTime? EndTime { get; set; }
        // Отработанные часы
        public double? HoursWorked { get; set; }
        // Внешний ключ на сотрудника
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
}
