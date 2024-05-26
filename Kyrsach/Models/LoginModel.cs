
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public class LoginModel
    {

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [Display(Name = "Remember?")]
        public bool RememberMe { get; set; }

        [Display(Name = "Login")]
        public string? Name { get; set; }

        [Display(Name = "Email")]
        public string? Email { get; set; }
                [Required(ErrorMessage ="Поле не заполнено")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
                [Required(ErrorMessage ="Поле не заполнено")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        public int? NumberStudentBook { get; set; }
        public int Group { get; set; }
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string? PasswordConfirm { get; set; }

        public string? Role { get; set; }
    }

}
