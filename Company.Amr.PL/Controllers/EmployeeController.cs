using System.Reflection.Metadata;
using AutoMapper;
using Company.Amr.BLL.Interfaces;
using Company.Amr.DAL.Models;
using Company.Amr.PL.Dtos;
using Company.Amr.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Amr.PL.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();

            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
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

        public async Task<IActionResult> Search(string Search)
        {
            var employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(Search);
            return PartialView("EmployeePartialView/EmployeesTablePartialView", employees);
        }


        [HttpGet]
        public IActionResult Create()
        {
            //var departments = _departmentRepository.GetAll();
            //ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if(model.Image is not null)
                model.ImageName = DocumentSettings.UploadFile(model.Image,"images");

                var employee = _mapper.Map<Employee>(model);
                await _unitOfWork.EmployeeRepository.AddAsync(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created !!";
                    return RedirectToAction(nameof(Index));
                };
            }


            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Employee With Id {id} is Not Found" });

            return View(viewName, employee);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            //var departments = _departmentRepository.GetAll();
            //ViewData["departments"] = departments;
            if (id is null) return BadRequest("Invalid Id");

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value);

            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Employee With Id {id} is Not Found" });

            var employeeDto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(employeeDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if (model.ImageName is not null && model.Image is not null )
                {
                    DocumentSettings.DeleteFile(model.ImageName,"images");
                }

                if(model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "images");
                }

                //if (id != employee.Id) return BadRequest("Invalid Id");
                var existingEmployee = await _unitOfWork.EmployeeRepository.GetAsync(id);
                if (existingEmployee == null)
                {
                    return NotFound(new { StatusCode = 404, Message = $"Employee With Id {id} is Not Found" });
                }

                var employee = _mapper.Map(model, existingEmployee);

                _unitOfWork.EmployeeRepository.Update(existingEmployee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0) return RedirectToAction(nameof(Index));
              
            }

            return View(model);
        }





        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id);
            if (employee == null)
            {
                return NotFound(new { StatusCode = 404, Message = $"Employee With Id {id} is Not Found" });
            }

            _unitOfWork.EmployeeRepository.Delete(employee);
            var count = await _unitOfWork.CompleteAsync();
            if (count > 0)
            {
                if(employee.ImageName is not null)
                DocumentSettings.DeleteFile(employee.ImageName,"images");
                return RedirectToAction(nameof(Index));
            }



            return View(employee);
        }
    }
}

