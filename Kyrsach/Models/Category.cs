using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public partial class Category
    {
        public Category()
        {
            Pairs = new List<Pair>();
            Tests = new List<Test>();
        }
        [Key]
        public int IdCategory { get; set; }

        [RegularExpression(@"^[а-яА-Я0-9,\.\s-]{6,15}$", ErrorMessage = "Некорректные данные. Название должно содержать от 3 до 15 русских символов, цифр, запятых, тире и точек")]
        [StringLength(15, MinimumLength = 3)]
        [Required(ErrorMessage ="Название не заполнено")]
        public string NameCategory { get; set; } = null!;
        public string WhoCreatedCategory { get; set; }

        public byte[]? Picture { get; set; }
        public string? Type { get; set; }

        public virtual List<Pair> Pairs { get; set; }
        public virtual List<Test> Tests { get; set; }
    }
}
