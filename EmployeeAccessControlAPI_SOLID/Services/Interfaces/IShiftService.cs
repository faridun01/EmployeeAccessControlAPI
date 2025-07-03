namespace EmployeeAccessControlAPI_SOLID.Services.Interfaces
{
    public interface IShiftService
    {
        string StartShift(int employeeId, DateTime startTime);
        string EndShift(int employeeId, DateTime endTime);
    }
}
