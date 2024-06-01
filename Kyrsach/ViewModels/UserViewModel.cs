using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public class UserViewModel : IdentityUser
    {
		[RegularExpression(@"^[а-яА-Я]{6,15}$", ErrorMessage = "Некорректные данные. Фамилия должна содержать от 3 до 15 русских букв")]
		[StringLength(15, MinimumLength = 3)]
		public string LastName { get; set; } = null!;
		[RegularExpression(@"^[а-яА-Я]{6,15}$", ErrorMessage = "Некорректные данные. Имя должно содержать от 3 до 15 русских букв")]
		[StringLength(15, MinimumLength = 3)]
		public string FirstName { get; set; } = null!;
		
		public int? NumberStudentBook { get; set; }
        public string? NameGroup { get; set; }
        public string NameRole { get; set; } = null!;
	}
}
