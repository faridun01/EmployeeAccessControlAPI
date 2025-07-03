using EmployeeAccessControlAPI_SOLID.Models;
using EmployeeAccessControlAPI_SOLID.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при получении сотрудника: {ex.Message}");
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
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при удалении сотрудника: {ex.Message}");
            }
        }
    }
}
