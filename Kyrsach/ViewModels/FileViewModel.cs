using System.ComponentModel.DataAnnotations;

namespace Kyrsach.ViewModels
{
    public class FileViewModel
    {
        public int id { get; set; }
        [RegularExpression(@"^[а-яА-Я0-9,\.\s-]{3,50}$", ErrorMessage = "Некорректные данные. Название должно содержать от 3 до 50 русских символов, цифр, запятых, тире и точек")]
        [StringLength(50, MinimumLength = 3)]
        [Required(ErrorMessage = "Поле не заполнено")]
        public string Name { get; set; }
        public string Path { get; set; }
        public byte[]? Data { get; set; }
        public string WhoCreated { get; set; }

    }
}
