using Kyrsach.Models;
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.ViewModels
{
    public class CategoryViewModel
    {


        public virtual List<Test>? Tests { get; set; }
        public int IdCategory { get; set; }

        [RegularExpression(@"^[а-яА-Я0-9,\.\s-]{3,15}$", ErrorMessage = "Некорректные данные. Название должно содержать от 3 до 15 русских символов, цифр, запятых, тире и точек")]
        [StringLength(15, MinimumLength = 3)]
        public string NameCategory { get; set; } = null!;
                [Required(ErrorMessage ="Поле не заполнено")]
        public string WhoCreatedCategory { get; set; } = null!;
		public IFormFile? Picture { get; set; }
        public string? Type { get; set; }
    }
}
