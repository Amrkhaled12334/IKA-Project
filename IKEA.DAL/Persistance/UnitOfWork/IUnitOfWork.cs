using IKEA.DAL.Persistance.Repositories.Departments;
using IKEA.DAL.Persistance.Repositories.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Persistance.UnitOfWork
{
    public interface IUnitOfWork
    {
        public  IDepartmentRepository departmentRepository { get; }
        public  IEmployeeRepository employeeRepository { get;}

        public Task<int> Complete();

        //public void Dispose();

    }
}
