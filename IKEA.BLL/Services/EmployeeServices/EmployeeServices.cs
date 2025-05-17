using IKEA.BLL.Common.Services.Attachments;
using IKEA.BLL.Dto_s.Employees;
using IKEA.DAL.Models.Employees;
using IKEA.DAL.Persistance.Repositories.Employees;
using IKEA.DAL.Persistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services.EmployeeServices
{
    public class EmployeeServices:IEmployeeServices
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IAttachmentServices attachmentServices;

        public EmployeeServices(IUnitOfWork unitOfWork,IAttachmentServices attachmentServices)
        {
            this.unitOfWork = unitOfWork;
            this.attachmentServices = attachmentServices;
        }
        public async Task<IEnumerable<EmployeeDto>> GetAllEmployees(string? search)
        {

            var Employees =  unitOfWork.employeeRepository.GetAll();

            var FilterdEmployees = Employees.Where(E => E.IsDeleted == false && (string.IsNullOrEmpty(search) ||E.Name.ToLower().Contains(search.ToLower()))).Include(E=>E.Department);
            var AfterFilteration = FilterdEmployees.Select(E => new EmployeeDto()
            {
                Id = E.Id,
                Name = E.Name,
                Age = E.Age,
                Salary = E.Salary,
                IsActive = E.IsActive,
                Email = E.Email,
                Gender = E.Gender,
                Department = E.Department.Name,
                EmployeeType = E.EmployeeType,
            });

            return await AfterFilteration.ToListAsync();
        }
        public async Task<EmployeeDetailsDto>? GetEmployeeById(int id)
        {
            var employee = await unitOfWork.employeeRepository.GetById(id);  

            if(employee is not null)
            {
                return new EmployeeDetailsDto()
                {
                    Id = employee.Id,
                    Name = employee.Name,
                    Age = employee.Age,
                    Address = employee.Address,
                    IsActive = employee.IsActive,
                    Salary = employee.Salary,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    HiringDate = employee.HiringDate,
                    Gender = employee.Gender,
                    Department = employee.Department?.Name,
                    EmployeeType = employee.EmployeeType,
                    LastModifiedBy = employee.LastModifiedBy,
                    CreatedBy = employee.CreatedBy,
                    LastModifiedOn = employee.LastModifiedOn,
                    CreatedOn = employee.CreatedOn,
                    ImageName = employee.ImageName,
                };
            }
            return null;
        }

        public async  Task<int> CreateEmployee(CreatedEmployeeDto employeeDto)
        {
            var Employee = new Employee()
            {
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                IsActive = employeeDto.IsActive,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                HiringDate = employeeDto.HiringDate,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                DepartmentId = employeeDto.DepartmentId,
                CreatedBy = 1,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.Now,
                CreatedOn = DateTime.Now,
            };//Image Name => String => Upload

            if(employeeDto.Image is not null)
            {
             Employee.ImageName =   attachmentServices.UploadImage(employeeDto.Image, "images");
            }


            unitOfWork.employeeRepository.Add(Employee);
            return await unitOfWork.Complete();
        }
        public async Task<int> UpdateEmployee(UpdatedEmployeeDto employeeDto)
        {
            var Employee = new Employee()
            {
                Id = employeeDto.Id,
                Name = employeeDto.Name,
                Age = employeeDto.Age,
                Address = employeeDto.Address,
                IsActive = employeeDto.IsActive,
                Salary = employeeDto.Salary,
                Email = employeeDto.Email,
                PhoneNumber = employeeDto.PhoneNumber,
                HiringDate = employeeDto.HiringDate,
                DepartmentId = employeeDto.DepartmentId,
                Gender = employeeDto.Gender,
                EmployeeType = employeeDto.EmployeeType,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.Now,
                ImageName = employeeDto.ImageName
            };

            if(employeeDto.Image is not null)
            {
                if(Employee.ImageName is not null)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "images", Employee.ImageName);
                    attachmentServices.DeleteImage(filePath);
                }
                Employee.ImageName = attachmentServices.UploadImage(employeeDto.Image, "images");
            }
            unitOfWork.employeeRepository.Update(Employee);
            return await unitOfWork.Complete();
        }
        public async Task<bool> DeleteEmployee(int id)
        {
            var employee = await unitOfWork.employeeRepository.GetById(id);



            if (employee is not null)
            {
                if(employee.ImageName is not null)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", "images", employee.ImageName);
                    attachmentServices.DeleteImage(filePath);
                }


                unitOfWork.employeeRepository.Delete(employee);
            }

            if (await unitOfWork.Complete() > 0)
                return true;
            else
                return false;
        }   
    }
}
