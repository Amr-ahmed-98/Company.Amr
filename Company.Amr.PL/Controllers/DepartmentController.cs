using Company.Amr.BLL.Interfaces;
using Company.Amr.BLL.Repositories;
using Company.Amr.DAL.Models;
using Company.Amr.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.Amr.PL.Controllers
{
    // MVC Controller
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        // ASK CLR Create Object From DepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet] // GET: /Department/Index
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();

            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if(ModelState.IsValid) // Server Side Validation
            {
                var department = new Department() 
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = DateTime.Now
                };
                
                var count = _departmentRepository.Add(department);
                if(count > 0) return RedirectToAction(nameof(Index));
            }


            return View();
        }
    }
}
