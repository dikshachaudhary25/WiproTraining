using Microsoft.EntityFrameworkCore;
namespace MyApiApp.Models;

public class EmpContext : DbContext
{
    public EmpContext(DbContextOptions<EmpContext> options) : base(options)
    {

    }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Book>Books { get; set; }
 
 }