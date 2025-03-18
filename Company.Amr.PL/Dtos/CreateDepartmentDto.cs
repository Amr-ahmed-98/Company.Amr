using System.ComponentModel.DataAnnotations;
using Company.Amr.DAL.Models;

namespace Company.Amr.PL.Dtos
{
    public class CreateDepartmentDto
    {
        [Required(ErrorMessage = "Code is required !" )]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required !")]
        public string Name { get; set; }

        [Required(ErrorMessage = "CreateAt is required !")]
        public DateTime CreateAt { get; set; }

        //public List<Employee> employees { get; set; }
    }
}
