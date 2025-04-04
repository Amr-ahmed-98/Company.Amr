using System.ComponentModel.DataAnnotations;

namespace Company.Amr.PL.Dtos
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]

        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(NewPassword), ErrorMessage = "Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
