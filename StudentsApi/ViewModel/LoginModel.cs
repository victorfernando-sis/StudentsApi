using System.ComponentModel.DataAnnotations;

namespace AlunosApi.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(20, ErrorMessage = "The {0} must have at least {2} and maximum " +
            "{1} characters.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}
