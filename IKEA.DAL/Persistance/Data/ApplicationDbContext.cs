using IKEA.DAL.Models.Departments;
using IKEA.DAL.Models.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Persistance.Data
{
    public class ApplicationDbContext:DbContext
    {//DI
        //Department => Context => Options

        //Ask CLR Generate Options for My Context
        public ApplicationDbContext(DbContextOptions options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
