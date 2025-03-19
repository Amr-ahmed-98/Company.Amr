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
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();

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
            if (ModelState.IsValid) // Server Side Validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt,
                };

                 _unitOfWork.DepartmentRepository.Add(department);
                var count = _unitOfWork.Complete();
                if (count > 0) return RedirectToAction(nameof(Index));
            }


            return View(model);
        }


        [HttpGet]
        public IActionResult Details(int? id , string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id"); // 400 Bad Request

            var department = _unitOfWork.DepartmentRepository.Get(id.Value);

            if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is Not Found" }); // 404 Not Found

            return View(viewName,department);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null) return BadRequest("Invalid Id"); // 400 Bad Request

            var department = _unitOfWork.DepartmentRepository.Get(id.Value);

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
        public IActionResult Edit([FromRoute] int id, CreateDepartmentDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != department.Id) return BadRequest("Invalid Id"); // 400
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt,
                };
                 _unitOfWork.DepartmentRepository.Update(department);
                var count = _unitOfWork.Complete();

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


        public IActionResult Delete(int? id)
        {
            //if (id is null) return BadRequest("Invalid Id"); // 400 Bad Request

            //var department = _departmentRepository.Get(id.Value);

            //if (department is null) return NotFound(new { StatusCode = 404, Message = $"Department With Id {id} is Not Found" }); // 404 Not Found

            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if (id != department.Id) return BadRequest("Invalid Id"); // 400
                 _unitOfWork.DepartmentRepository.Delete(department);
                var count = _unitOfWork.Complete();
                if (count > 0) return RedirectToAction(nameof(Index));

            }

            return View(department);
        }

    }
}
