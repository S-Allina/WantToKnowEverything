using System.ComponentModel.DataAnnotations;

namespace Kyrsach.ViewModels
{
    public class PairViewModel
    {
        public int IdPair { get; set; }
        public int IdCategory { get; set; }
        [RegularExpression(@"^[а-яА-ЯA-Za-z0-9,{}='\.\s-]{3,15}$", ErrorMessage = "Некорректные данные. Поле должно содержать от 3 до 15  символов, цифр, запятых, тире и точек")]
        [StringLength(15, MinimumLength = 3)]
        public string? Card1Text { get; set; }
        [RegularExpression(@"^[а-яА-ЯA-Za-z0-9,{}='\.\s-]{3,15}$", ErrorMessage = "Некорректные данные. Поле должно содержать от 3 до 15  символов, цифр, запятых, тире и точек")]
        [StringLength(15, MinimumLength = 3)]
        public string? Card2Text { get; set; }
        public IFormFile? Card1Img { get; set; }
        public IFormFile? Card2Img { get; set; }

    }
}

