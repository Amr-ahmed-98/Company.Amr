using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Company.Amr.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Amr.DAL.Data.Contexts
{
    // CLR Will Create Object from CompanyDbContext
    public class CompanyDbContext : DbContext
    {
        public DbSet<Department> departments { get; set; }
        public DbSet<Employee> employees { get; set; }

        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server = AMRAHMED\\MSSQLSERVER01; Database = CompanyDb; Trusted_Connection = True ; TrustServerCertificate=True");
        //}
    }
}
