using AutoMapper;
using IKEA.BLL.Dto_s.Departments;
using IKEA.BLL.Services.DepartmentServices;
using IKEA.PL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IKEA.PL.Controllers
{
    //Inhertance : DepartmentController is a Controller
    //Composition: DepartmentController has a IDepartmentService
    public class DepartmentController : Controller
    {
        #region Services - DI
        private readonly IDepartmentServices departmentServices;
        private readonly IMapper mapper;
        private readonly ILogger<DepartmentController> logger;
        private readonly IWebHostEnvironment environment;

        public DepartmentController(IDepartmentServices _departmentServices,IMapper mapper, ILogger<DepartmentController> _logger, IWebHostEnvironment environment)
        {
            departmentServices = _departmentServices;
            this.mapper = mapper;
            logger = _logger;
            this.environment = environment;
        }
        #endregion

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //ViewData Dictionary => Key Value Pair
            //ViewBag 
            //TempData

            var Departments = await departmentServices.GetAllDepartments();
            //ViewData["Message"] = "Hello From ViewData";//Life Time per Request => Key Value Pair=> Required TypeSefity 
            //ViewData["TEST"] = "Hello From ViewData";//Life Time per Request => Key Value Pair=> Required TypeSefity 

            //string Message = ViewData["Message"] as string;
            //ViewBag.Message = "Hello From ViewBag" ;
            //ViewBag.Test = "Hello From ViewBag";//Life Time per Request => Dynamic 
            //Life Time per Request => Dynamic 

            //string Message = ViewBag.Message;

            return View(Departments);
        }
        #endregion

        #region Details
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return BadRequest();

            var department = await departmentServices.GetDepartmentById(id.Value);

            if (department is null)
                return NotFound();

            return View(department);




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
        public async Task<IActionResult> Create(DepartmentVM departmentVM)
        {
            //ServerSide Validation
            if (!ModelState.IsValid)
                return View(departmentVM);

            var Message = string.Empty;
            try
            {
                var departmentDto = mapper.Map<DepartmentVM,CreatedDepartmentDto>(departmentVM);
                //var departmentDto = new CreatedDepartmentDto()
                //{
                //    Name = departmentVM.Name,
                //    Code = departmentVM.Code,
                //    CreationDate = departmentVM.CreationDate,
                //    Description = departmentVM.Description,
                //};
                var Result = await departmentServices.CreateDepartment(departmentDto);
                if (Result > 0)
                {
                    TempData["Message"] = $"{departmentDto.Name} Department is Created";
                    return RedirectToAction(nameof(Index));
                }
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
            return View(departmentVM);
        }
        #endregion

        #region Update
        [HttpGet] //Get: /Department/Edit/10
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null)
                return BadRequest();

            var Department = await departmentServices.GetDepartmentById(id.Value);

            if (Department is null)
                return NotFound();

            //var MappedDepartment = new DepartmentVM()
            //{
            //    Id = Department.Id,
            //    Name = Department.Name,
            //    Code = Department.Code,
            //    Description = Department.Description,
            //    CreationDate = Department.CreationDate,
            //};
            var MappedDepartment = mapper.Map<DepartmentDetailsDto,DepartmentVM>(Department);
            return View(MappedDepartment);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentVM departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);

            var Message = String.Empty;
            try
            {
                //var departmentDto = new UpdatedDepartmentDto()
                //{
                //    Id = departmentVM.Id,
                //    Name = departmentVM.Name,
                //    Code = departmentVM.Code,
                //    CreationDate = departmentVM.CreationDate,
                //    Description = departmentVM.Description,
                //};

                var departmentDto = mapper.Map<DepartmentVM, UpdatedDepartmentDto>(departmentVM);
                var Result = await departmentServices.UpdateDepartment(departmentDto);

                if (Result > 0)
                    return RedirectToAction(nameof(Index));
                else
                    Message = "Department is Not Updated";
            }
            catch (Exception ex)
            {
                //1.log Exceptions 
                logger.LogError(ex, ex.Message);

                //2.Set Message

                Message = environment.IsDevelopment() ? ex.Message : "An Error has been occured during Update the Department!";
            }

            ModelState.AddModelError(string.Empty, Message);
            return View(departmentVM);
        }




        #endregion

        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null)
                return BadRequest();

            var Department = await departmentServices.GetDepartmentById(id.Value);

            if (Department is null)
                return NotFound();

            return View(Department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int DeptId)
        {
            var Message = string.Empty;
            try
            {
               var IsDeleted =await  departmentServices.DeleteDepartment(DeptId);

                if(IsDeleted)
                    return RedirectToAction(nameof(Index));

                Message = "Department is Not Deleted";

            }
            catch (Exception ex)
            {
                // 1. Log Exception
                logger.LogError(ex, ex.Message);

                // 2. Set Message

                Message = environment.IsDevelopment() ? ex.Message : "An Error has been occured during delete the Department!";
            }

            ModelState.AddModelError(string.Empty, Message);
            return RedirectToAction(nameof(Delete),new {id = DeptId});
        }
        #endregion

    }
}
