
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public class RegisterUserModel
    {

        [Required(ErrorMessage = "Поле не заполнено")]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Поле не заполнено")]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
        //[MaxLength(6,ErrorMessage ="Номер зачётки должен содержать 6 цифр")]
        public int? NumberStudentBook { get; set; }
        public int? Group { get; set; }
    }

}
