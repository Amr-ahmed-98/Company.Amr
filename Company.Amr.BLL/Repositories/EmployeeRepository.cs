using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Amr.BLL.Interfaces;
using Company.Amr.DAL.Data.Contexts;
using Company.Amr.DAL.Models;

namespace Company.Amr.BLL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetAll()
        {
            return _context.employees.ToList();
        }
        public Employee? Get(int id)
        {
            return _context.employees.Find(id);
        }


        public int Add(Employee model)
        {
            _context.employees.Add(model);
            return _context.SaveChanges();
        }
        public int Update(Employee model)
        {
            _context.employees.Update(model);
            return _context.SaveChanges();
        }

        public int Delete(Employee model)
        {
            _context.employees.Remove(model);
            return _context.SaveChanges();
        }
    }
}
