using System.ComponentModel.DataAnnotations;

namespace EmployeeAccessControlAPI_SOLID.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public required string FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public Position Position { get; set; }

        public List<Shift> Shifts { get; set; } = new();
    }
}
