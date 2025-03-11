using Company.Amr.BLL.Interfaces;
using Company.Amr.DAL.Models;
using Company.Amr.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.Amr.PL.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IEmployeeRepository _employeeRepository;


        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAll();

            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    Name = model.Name,
                    Age = model.Age,
                    Email = model.Email,
                    Address = model.Address,
                    Phone = model.Phone,
                    Salary = model.Salary,
                    CreateAt = model.CreateAt,
                    HiringDate = model.HiringDate,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                };

                var count = _employeeRepository.Add(employee);
                if (count > 0) return RedirectToAction(nameof(Index));
            }


            return View();
        }


        [HttpGet]
        public IActionResult Details(int? id, string viewName = "Details")
        {
            if (id is null) return BadRequest("Invalid Id");

            var employee = _employeeRepository.Get(id.Value);

            if (employee is null) return NotFound(new { StatusCode = 404, Message = $"Employee With Id {id} is Not Found" });

            return View(viewName, employee);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {

            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (id != employee.Id) return BadRequest("Invalid Id");
                var count = _employeeRepository.Update(employee);

                if (count > 0) return RedirectToAction(nameof(Index));

            }

            return View(employee);
        }





        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee employee)
        {
            if (ModelState.IsValid)
            {
                if (id != employee.Id) return BadRequest("Invalid Id");
                var count = _employeeRepository.Delete(employee);

                if (count > 0) return RedirectToAction(nameof(Index));

            }

            return View(employee);
        }
    }
}

