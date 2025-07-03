using EmployeeAccessControlAPI_SOLID.Models;
using EmployeeAccessControlAPI_SOLID.Services;
using EmployeeAccessControlAPI_SOLID.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAccessControlAPI_SOLID.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HRController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public HRController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("AddEmployee")]
        public IActionResult AddEmployee([FromBody] Employee employee)
        {
            if (employee == null)
                return BadRequest("Некорректные данные сотрудника.");
            try
            {
                var addedEmployee = _employeeService.AddEmployee(employee);
                return CreatedAtAction(nameof(GetEmployee), new { id = addedEmployee.Id }, addedEmployee);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при добавлении сотрудника: {ex.Message}");
            }
        }

        [HttpGet("GetEmployee/{id}")]
        public IActionResult GetEmployee(int id)
        {
            if (id <= 0)
                return BadRequest("Некорректный ID сотрудника.");

            try
            {
                var employee = _employeeService.GetEmployee(id);

                if (employee == null)
                    return NotFound($"Сотрудник с ID {id} не найден.");

                var result = new
                {
                    employee.Id,
                    employee.FirstName,
                    employee.LastName,
                    employee.MiddleName,
                    employee.Position,
                    Shifts = employee.Shifts?.Select(s => new
                    {
                        s.Id,
                        StartTime = s.StartTime?.ToString("yyyy-MM-dd HH:mm"),
                        EndTime = s.EndTime?.ToString("yyyy-MM-dd HH:mm"),
                        s.HoursWorked
                    }).ToList()
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка сервера при получении сотрудника: {ex.Message}");
            }
        }


        [HttpPut("UpdateEmployee")]
        public IActionResult UpdateEmployee([FromBody] Employee employee)
        {
            if (employee == null || employee.Id <= 0)
                return BadRequest("Некорректные данные сотрудника.");
            try
            {
                var updatedEmployee = _employeeService.UpdateEmployee(employee);
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при обновлении сотрудника: {ex.Message}");
            }
        }

        [HttpDelete("DeleteEmployee/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            if (id <= 0)
                return BadRequest("Некорректный ID сотрудника.");
            try
            {
                _employeeService.DeleteEmployee(id);
                return Ok($"Сотрудник под ID {id} удалён");
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при удалении сотрудника: {ex.Message}");
            }
        }

        [HttpPost("GenerateShifts")]
        public IActionResult GenerateMonthlyShifts()
        {
            _employeeService.GenerateShiftsForCurrentMonth();
            return Ok("Смены успешно созданы.");
        }

        [HttpGet("GetAllEmployeeViolations")]
        public IActionResult GetAllEmployeeViolations()
        {
            try
            {
                var result = _employeeService.GetEmployeesWithViolationCounts();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при получении нарушений: {ex.Message}");
            }
        }



    }
}
