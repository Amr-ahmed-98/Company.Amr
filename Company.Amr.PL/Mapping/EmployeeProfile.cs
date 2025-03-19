using AutoMapper;
using Company.Amr.DAL.Models;
using Company.Amr.PL.Dtos;

namespace Company.Amr.PL.Mapping
{
    //CLR
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeDto, Employee>();
            //.ForMember(d => d.Name , o => o.MapFrom(s => s.EmployeeName));
            CreateMap<Employee, CreateEmployeeDto>();
                //.ForMember(d => d.DepartmentName , o => o.MapFrom(s => s.Department.Name));
        }
    }
}
