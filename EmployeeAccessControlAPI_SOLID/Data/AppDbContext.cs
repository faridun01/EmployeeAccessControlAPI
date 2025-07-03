using EmployeeAccessControlAPI_SOLID.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeAccessControlAPI_SOLID.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Shift> Shifts { get; set; }
    }
}
