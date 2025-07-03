using EmployeeAccessControlAPI_SOLID.Services;
using EmployeeAccessControlAPI_SOLID.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAccessControlAPI_SOLID.Controllers
{
    // EmployeeAccessControlAPI_SOLID.Controllers
    // This controller handles gate operations for employee access control.
    // It is responsible for rendering the gate view.
    [Route("api/kpp")]
    [ApiController]
    public class GateController : Controller
    {
        private readonly IShiftService shiftService;
        public GateController(IShiftService shiftService)
        {
            this.shiftService = shiftService;
        }
        // Метод для отметки входа (StartShift)
        // Метод для отметки входа (StartShift)
        [HttpPost("StartShift")]
        public IActionResult StartShift(int employeeId, DateTime startTime)
        {
            if (employeeId <= 0)
                return BadRequest("Некорректный ID сотрудника.");

            var result = shiftService.StartShift(employeeId, startTime);
            if (result.StartsWith("Ошибка") || result.Contains("не найден"))
                return BadRequest(result);

            return Ok(result);
        }

        // Метод для отметки выхода (EndShift)
        [HttpPost("EndShift")]
        public IActionResult EndShift(int employeeId, DateTime endTime)
        {
            if (employeeId <= 0)
                return BadRequest("Некорректный ID сотрудника.");

            var result = shiftService.EndShift(employeeId, endTime);
            if (result.StartsWith("Ошибка") || result.Contains("не найден"))
                return BadRequest(result);

            return Ok(result);
        }
    }
}
