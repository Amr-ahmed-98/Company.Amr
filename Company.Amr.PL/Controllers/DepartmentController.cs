using Company.Amr.BLL.Interfaces;
using Company.Amr.BLL.Repositories;
using Company.Amr.DAL.Models;
using Company.Amr.PL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Amr.PL.Controllers
{
    // MVC Controller
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRepository;

        // ASK CLR Create Object From DepartmentRepository
        public DepartmentController(
            //IDepartmentRepository departmentRepository
            IUnitOfWork unitOfWork
            )
        {
            _unitOfWork = unitOfWork;
            //_departmentRepository = departmentRepository;
        }

        [HttpGet] // GET: /Department/Index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt,
                };

                 await _unitOfWork.DepartmentRepository.AddAsync(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0) return RedirectToAction(nameof(Index));
            }


            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id , string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id"); // 400 Bad Request

            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);

            if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is Not Found" }); // 404 Not Found

            return View(viewName,department);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id"); // 400 Bad Request

            var department = await _unitOfWork.DepartmentRepository.GetAsync(id.Value);

            if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is Not Found" }); // 404 Not Found

            var departmentDto = new CreateDepartmentDto()
            {
                Code = department.Code,
                Name = department.Name,
                CreateAt = department.CreateAt,
            };

            return View(departmentDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                var existingDepartment = await _unitOfWork.DepartmentRepository.GetAsync(id);

                if (existingDepartment is null)
                {
                    return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is Not Found" });
                }

                existingDepartment.Name = model.Name;
                existingDepartment.Code = model.Code;
                existingDepartment.CreateAt = model.CreateAt;

                _unitOfWork.DepartmentRepository.Update(existingDepartment);
                var count = await _unitOfWork.CompleteAsync();

                if (count > 0) return RedirectToAction(nameof(Index));

            }

            return View(model);
        }


        // another senario to handle the invalid data that come from the user

        //[HttpPost]
        //[ValidateAntiForgeryToken] // this attribute use with action that has post method (Edit , Create)
        //// another senario to handle the invalid data that come from the user
        //public IActionResult Edit([FromRoute] int id, UpdateDepartmentDto model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var department = new Department()
        //        {
        //            Id = id,
        //            Name = model.Name,
        //            Code = model.Code,
        //            CreateAt = model.CreateAt,
        //        };
        //        var count = _departmentRepository.Update(department);

        //        if (count > 0) return RedirectToAction(nameof(Index));

        //    }

        //    return View(model);
        //}

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id"); // 400 Bad Request

            //var department = _departmentRepository.Get(id.Value);

            //if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is Not Found" }); // 404 Not Found

            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var department = await _unitOfWork.DepartmentRepository.GetAsync(id);

            if (department is null)
            {
                return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is Not Found" });
            }

            if (id != department.Id) return BadRequest("Invalid Id"); // 400
                 _unitOfWork.DepartmentRepository.Delete(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0) return RedirectToAction(nameof(Index));

            

            return View(department);
        }

    }
}
