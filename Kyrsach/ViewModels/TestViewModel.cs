using Kyrsach.Models;
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.ViewModels
{
    public class TestViewModel
    {
        public TestViewModel()
        {
            Questions = new HashSet<Question>();
        }

        public int IdTest { get; set; }
        public int IdCategory { get; set; }
        [RegularExpression(@"^[а-яА-ЯA-Za-z0-9,#\.\s-]{2,15}$", ErrorMessage = "Некорректные данные. " +
              "Название должно содержать от 2 до 15 символов, цифр, запятых, тире и точек")]
        [Required(ErrorMessage = "Поле не заполнено")]
        public string NameTest { get; set; } = null!;
        public int? WhoAnsweredTest { get; set; }

        public virtual Category IdCategoryNavigation { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; }
    }
}
