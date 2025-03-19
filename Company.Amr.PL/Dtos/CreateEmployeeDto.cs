using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Company.Amr.DAL.Models;

namespace Company.Amr.PL.Dtos
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Range(18, 60, ErrorMessage = "Age must be between 18 and 60")]
        public int? Age { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [RegularExpression(@"[0-9]{1,3}-[a-zA-Z]{5,10}-[a-zA-Z]{4,10}-[a-zA-Z]{5,10}$", ErrorMessage = "Address Must be in this format: 123-StreetName-City-Country")]
        public string Address { get; set; }

        [DataType(DataType.PhoneNumber, ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; }

        [DataType(DataType.Currency, ErrorMessage = "Invalid Salary")]
        public decimal Salary { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        [DisplayName("Hiring Date")]
        public DateTime HiringDate { get; set; }
        [DisplayName("Date of Creation")]
        public DateTime CreateAt { get; set; }

        public int? DepartmentId { get; set; }

        public string? DepartmentName { get; set; }

        public string? ImageName { get; set; }
        public IFormFile? Image { get; set; }

    }
}
