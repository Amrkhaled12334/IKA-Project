using IKEA.DAL.Common.Enums;
using IKEA.DAL.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Models.Employees
{
    public class Employee:ModelBase
    {
        public string Name { get; set; }    

        public int? Age { get; set; }

        public string? Address { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public DateOnly HiringDate { get; set; }

        public Gender Gender { get; set; }

        public EmployeeType EmployeeType { get; set; }

        public int? DepartmentId { get; set; }

        //Navigtional Property [One]
        public virtual Department? Department { get; set; }

        public string? ImageName {  get; set; }
    }
}
