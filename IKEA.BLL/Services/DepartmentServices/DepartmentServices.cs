using IKEA.BLL.Dto_s.Departments;
using IKEA.DAL.Models.Departments;
using IKEA.DAL.Persistance.Repositories.Departments;
using IKEA.DAL.Persistance.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services.DepartmentServices
{
    public class DepartmentServices:IDepartmentServices
    {
        //Controller => Service => Repository => Context => Options 

        // Repository
       private IUnitOfWork unitOfWork;

        public DepartmentServices(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<DepartmentDto>> GetAllDepartments()
        {
            var Departments = await unitOfWork.departmentRepository.GetAll().Where(D=>!D.IsDeleted).Select(dept => new DepartmentDto()
            {
                Id = dept.Id,
                Name = dept.Name,
                Code = dept.Code,
                CreationDate = dept.CreationDate,
            }).ToListAsync();

            return Departments;

            //List<DepartmentDto> departmentDtos = new List<DepartmentDto>();

            //foreach(var dept in Departments)
            //{
            //    DepartmentDto departmentDto = new DepartmentDto()
            //    {
            //        Id = dept.Id,
            //        Name = dept.Name,
            //        Code = dept.Code,   
            //        CreationDate = dept.CreationDate,
            //    };
            //    departmentDtos.Add(departmentDto);
            //}
            //Auto Mapper
        }
        public async Task<DepartmentDetailsDto>? GetDepartmentById(int id)
        {
            var Department = await unitOfWork.departmentRepository.GetById(id);

            if (Department is not null)
                return new DepartmentDetailsDto()
                {
                    Id =Department.Id,
                    Name = Department.Name,
                    Code = Department.Code,
                    Description = Department.Description,
                    CreationDate = Department.CreationDate,
                    IsDeleted = Department.IsDeleted,
                    LastModifiedBy = Department.LastModifiedBy,
                    LastModifiedOn = Department.LastModifiedOn,
                    CreatedBy = Department.CreatedBy,
                    CreatedOn = Department.CreatedOn,
                };

            return null;

        }

        public async Task<int> CreateDepartment(CreatedDepartmentDto departmentDto)
        {
            var CreatedDepartment = new Department()
            {
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,
                CreatedBy = 1,
                CreatedOn = DateTime.Now,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.Now,
            };
            unitOfWork.departmentRepository.Add(CreatedDepartment);
            return await unitOfWork.Complete();
        }
        public async Task<int> UpdateDepartment(UpdatedDepartmentDto departmentDto)
        {
            var UpdatedDepartment = new Department()
            {
                Id = departmentDto.Id,
                Code = departmentDto.Code,
                Name = departmentDto.Name,
                Description = departmentDto.Description,
                CreationDate = departmentDto.CreationDate,
                LastModifiedBy = 1,
                LastModifiedOn = DateTime.Now,
            };
            unitOfWork.departmentRepository.Update(UpdatedDepartment);
            return await unitOfWork.Complete();
        }
        public  async Task<bool> DeleteDepartment(int id)
        {
            var department = await unitOfWork.departmentRepository.GetById(id);

            

            if(department is not null)
              unitOfWork.departmentRepository.Delete(department);

            if (await unitOfWork.Complete() > 0)
                return true;
            else
                return false;
        }
    }
}
