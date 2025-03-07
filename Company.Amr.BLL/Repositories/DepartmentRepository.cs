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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly CompanyDbContext _context; // Null

        public DepartmentRepository()
        {
            _context = new CompanyDbContext();
        }


        public IEnumerable<Department> GetAll()
        {
            return _context.departments.ToList();
        }
        public Department? Get(int id)
        {
            return _context.departments.Find(id);
        }
        public int Add(Department model)
        {
            _context.departments.Add(model);
            return _context.SaveChanges();
        }
        public int Update(Department model)
        {
            _context.departments.Update(model);
            return _context.SaveChanges();
        }

        public int Delete(Department model)
        {
            _context.departments.Remove(model);
            return _context.SaveChanges();
        }



    }
}
