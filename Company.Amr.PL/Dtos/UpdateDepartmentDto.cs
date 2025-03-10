using System.ComponentModel.DataAnnotations;

namespace Company.Amr.PL.Dtos
{
    public class UpdateDepartmentDto
    {
        [Required(ErrorMessage = "Code is required !")]
        public string Code { get; set; }

        [Required(ErrorMessage = "Name is required !")]
        public string Name { get; set; }

        [Required(ErrorMessage = "CreateAt is required !")]
        public DateTime CreateAt { get; set; }
    }
}
