using IKEA.BLL.Dto_s.Employees;
using IKEA.BLL.Services.DepartmentServices;
using IKEA.BLL.Services.EmployeeServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IKEA.PL.Controllers
{
    public class EmployeeController : Controller
    {
        #region Services - DI
        private readonly IEmployeeServices employeeServices;
        private readonly ILogger<EmployeeController> logger;
        private readonly IWebHostEnvironment environment;

        public EmployeeController(IEmployeeServices employeeServices, ILogger<EmployeeController> logger, IWebHostEnvironment environment)
        {
            this.employeeServices = employeeServices;
            this.logger = logger;
            this.environment = environment;
        }
        #endregion

        #region Index

        [HttpGet] // /Employee/Index
        public async Task<IActionResult> Index(string? search)
        {
            var Employees = await employeeServices.GetAllEmployees(search);
            return View(Employees);
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest();

            var employee = await employeeServices.GetEmployeeById(id.Value);

            if (employee is null)
                return NotFound();

            return View(employee);
        } 
        #endregion

        #region Create
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatedEmployeeDto EmployeeDto)
        {
            //ServerSide Validation
            if (!ModelState.IsValid)//false => BadRequest
            {
                return View(EmployeeDto);
            }

            var Message = string.Empty;
            try
            {
                var Result = await employeeServices.CreateEmployee(EmployeeDto);
                if (Result > 0)
                    return RedirectToAction(nameof(Index));
                else
                    Message = "Department is not Created";
                 
                
            }
            catch (Exception ex)
            {
                //1.Log Exception Kestral
                logger.LogError(ex, ex.Message);

                //2.Set Default Message User
                if (environment.IsDevelopment())
                    Message = ex.Message;             
                else
                    Message = "An Error Effect at The Creation Operator";
                  
                

            }
            ModelState.AddModelError(string.Empty, Message);
            return View(EmployeeDto);
        }
        #endregion

        #region Update
        [HttpGet] //Get: /Department/Edit/10
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest();

            var Employee = await employeeServices.GetEmployeeById(id.Value);

            if (Employee is null)
                return NotFound();

            var MappedEmployee = new UpdatedEmployeeDto()
            {
                Id = Employee.Id,
                Name = Employee.Name,
                Age = Employee.Age,
                Address = Employee.Address,
                HiringDate = Employee.HiringDate,
                Email = Employee.Email,
                PhoneNumber = Employee.PhoneNumber,
                Salary = Employee.Salary,
                Gender = Employee.Gender,
                EmployeeType = Employee.EmployeeType,
                IsActive = Employee.IsActive,
                ImageName = Employee.ImageName,
            };
            return View(MappedEmployee);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdatedEmployeeDto employeeDto)
        {
            if (!ModelState.IsValid)
                return View(employeeDto);

            var Message = String.Empty;
            try
            {
                var Result = await employeeServices.UpdateEmployee(employeeDto);

                if (Result > 0)
                    return RedirectToAction(nameof(Index));
                else
                    Message = "Employee is Not Updated";
            }
            catch (Exception ex)
            {
                //1.log Exceptions 
                logger.LogError(ex, ex.Message);

                //2.Set Message

                Message = environment.IsDevelopment() ? ex.Message : "An Error has been occured during Update the Employee!";
            }

            ModelState.AddModelError(string.Empty, Message);
            return View(employeeDto);
        }




        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var Employee = await employeeServices.GetEmployeeById(id.Value);

            if (Employee is null)
                return NotFound();

            return View(Employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int EmpId)
        {
            var Message = string.Empty;
            try
            {
                var IsDeleted = await employeeServices.DeleteEmployee(EmpId);

                if (IsDeleted)
                    return RedirectToAction(nameof(Index));

                Message = "Employee is Not Deleted";

            }
            catch (Exception ex)
            {
                // 1. Log Exception
                logger.LogError(ex, ex.Message);

                // 2. Set Message

                Message = environment.IsDevelopment() ? ex.Message : "An Error has been occured during delete the Employee!";
            }

            ModelState.AddModelError(string.Empty, Message);
            return RedirectToAction(nameof(Delete), new { id = EmpId });
        }
        #endregion
    }
}
