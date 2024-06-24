using Kyrsach.Models;
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.ViewModels
{
    public class CategoryViewModel
    {


        public virtual List<Test>? Tests { get; set; }
        public int IdCategory { get; set; }

        [RegularExpression(@"^[а-яА-ЯA-Za-z0-9,#\.\s-]{2,15}$", ErrorMessage = "Некорректные данные. " +
              "Название должно содержать от 2 до 15 символов, цифр, запятых, тире и точек")]
        [Required(ErrorMessage = "Поле не заполнено")]
        public string NameCategory { get; set; } = null!;
                
        public string WhoCreatedCategory { get; set; } = null!;
		public IFormFile? Picture { get; set; }
        public string? Type { get; set; }
    }
}
