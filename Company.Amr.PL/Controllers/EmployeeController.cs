using System.Reflection.Metadata;
using AutoMapper;
using Company.Amr.BLL.Interfaces;
using Company.Amr.DAL.Models;
using Company.Amr.PL.Dtos;
using Company.Amr.PL.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Company.Amr.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepository employeeRepository, 
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();

            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.GetByName(SearchInput);
            }
            //// Memory of View it like Dictionary
            //// To Access the Data that not related to the data that you send to view you have 3 properties
            //// 1. ViewData : Transfer extra information From Controller (Action) To View
            //// how From ViewData access Dictionary ?
            //ViewData["Message"] = "Hello From ViewData";

            //// 2. ViewBag  : Transfer extra information From Controller (Action) To View
            //// how From ViewBag access Dictionary ?
            ////ViewBag.Message = "Hello From ViewBag";
            //ViewBag.Message = new {Message = "Hello From ViewBag" };

            // Diffrent From ViewData and ViewBag ?
            // syntax are diffrent
            // Deal with ViewBag more flexible


            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            //var departments = _departmentRepository.GetAll();
            //ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if(model.Image is not null)
                model.ImageName = DocumentSettings.UploadFile(model.Image,"images");

                var employee = _mapper.Map<Employee>(model);
                _unitOfWork.EmployeeRepository.Add(employee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created !!";
                    return RedirectToAction(nameof(Index));
                };
            }


            return View(model);
        }


        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);

            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Employee With Id {id} is Not Found" });

            return View(viewName, employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            //var departments = _departmentRepository.GetAll();
            //ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");

            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);

            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Employee With Id {id} is Not Found" });

            //var employeeDto = new CreateEmployeeDto()
            //{

            //    Name = employee.Name,
            //    Age = employee.Age,
            //    Email = employee.Email,
            //    Address = employee.Address,
            //    Phone = employee.Phone,
            //    Salary = employee.Salary,
            //    CreateAt = employee.CreateAt,
            //    HiringDate = employee.HiringDate,
            //    IsActive = employee.IsActive,
            //    IsDeleted = employee.IsDeleted,
            //};
            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                //if (id != employee.Id) return BadRequest("Invalid Id");
                var existingEmployee = _unitOfWork.EmployeeRepository.Get(id);
                if (existingEmployee == null)
                {
                    return NotFound(new { StatusCode = 404, Message = $"Employee With Id {id} is Not Found" });
                }
                var employee = _mapper.Map(model, existingEmployee);

                _unitOfWork.EmployeeRepository.Update(existingEmployee);
                var count = _unitOfWork.Complete();
                if (count > 0) return RedirectToAction(nameof(Index));

            }

            return View(model);
        }





        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id)
        {

            var employee = _unitOfWork.EmployeeRepository.Get(id);
            if (employee == null)
            {
                return NotFound(new { StatusCode = 404, Message = $"Employee With Id {id} is Not Found" });
            }

            _unitOfWork.EmployeeRepository.Delete(employee);
            var count = _unitOfWork.Complete();
            if (count > 0) return RedirectToAction(nameof(Index));



            return View(employee);
        }
    }
}

