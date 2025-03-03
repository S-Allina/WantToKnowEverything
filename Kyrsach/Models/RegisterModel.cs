﻿
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public class RegisterModel
    {

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
                [Required(ErrorMessage ="Поле не заполнено")]
        public string? Password { get; set; }
        

        [Display(Name = "Login")]
        [Required(ErrorMessage = "Поле не заполнено")]
        public string? Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Поле не заполнено")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        //[MaxLength(6,ErrorMessage ="Номер зачётки должен содержать 6 цифр")]
        public int? NumberStudentBook { get; set; }
        public int? Group { get; set; }
        [Compare("Password", ErrorMessage = "Пароли не совподают")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Поле не заполнено")]
        public string? PasswordConfirm { get; set; }

        public string? Role { get; set; }
    }

}
