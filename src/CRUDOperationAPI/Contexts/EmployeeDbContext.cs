using CRUDOperationAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUDOperationAPI.Contexts
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
            :base(options)
        {

        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<EmployeeProject> EmployeeProject { get; set; }
        public DbSet<EmployeeSchedule> EmployeeSchedule { get; set; }
        public DbSet<ClientProject> ClientProject { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Departments> Departments { get; set; }
        public DbSet<DepartmentEmployee> DepartmentEmployee { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Leaves> Leaves { get; set; }






    }
}
