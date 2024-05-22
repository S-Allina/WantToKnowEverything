using Kyrsach.Models;
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.ViewModels
{
    public class CategoryViewModel
    {


        public virtual List<Test>? Tests { get; set; }
        public int IdCategory { get; set; }

        [StringLength(30, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 30 символов")]
        public string NameCategory { get; set; } = null!;
        public string WhoCreatedCategory { get; set; }
        public IFormFile? Picture { get; set; }
        public string? Type { get; set; }
    }
}
