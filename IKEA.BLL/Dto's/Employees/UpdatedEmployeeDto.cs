using IKEA.DAL.Common.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Dto_s.Employees
{
    public class UpdatedEmployeeDto
    {
        public int Id { get; set; }
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

        [Display(Name ="Department")]
        public int? DepartmentId { get; set; }

        public string? ImageName { get; set; }

        public IFormFile? Image {  get; set; }


    }
}
