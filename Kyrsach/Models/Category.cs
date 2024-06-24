using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public partial class Category
    {
        [Key]
        public int IdCategory { get; set; }

        [RegularExpression(@"^[а-яА-ЯA-Za-z0-9,#\.\s-]{2,15}$", ErrorMessage = "Некорректные данные. " +
            "Название должно содержать от 2 до 15 символов, цифр, запятых, тире и точек")]
        [StringLength(15, MinimumLength = 2)]
        [Required(ErrorMessage ="Название не заполнено")]
        public string NameCategory { get; set; } = null!;
        public string WhoCreatedCategory { get; set; }
        public byte[]? Picture { get; set; }
        public string? Type { get; set; }

        public virtual List<Pair>? Pairs { get; set; }
        public virtual List<Test>? Tests { get; set; }
    }
}
