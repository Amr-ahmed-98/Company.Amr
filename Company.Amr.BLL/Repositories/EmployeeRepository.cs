using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Amr.BLL.Interfaces;
using Company.Amr.DAL.Data.Contexts;
using Company.Amr.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Amr.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee> , IEmployeeRepository
    {
        private readonly CompanyDbContext _context;

        public EmployeeRepository(CompanyDbContext context) : base(context) // ASK CLR To Create Object from CompanyDbContext
        {
            _context = context;
        }

        public async Task<List<Employee>> GetByNameAsync(string name)
        {
           return await _context.employees.Where(E => E.Name.ToLower().Contains(name.ToLower())).Include(E => E.Department).ToListAsync();
        }
    }
}
