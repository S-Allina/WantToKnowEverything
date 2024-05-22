using Kyrsach.Models;
using System.ComponentModel.DataAnnotations;

namespace Kyrsach.ViewModels
{
    public class QuestionViewModel
    {

        public QuestionViewModel()
        {
            Answers = new List<Answer>();
        }

        public int IdQuestion { get; set; }
        public int IdTest { get; set; }

        [Required(ErrorMessage = "Необходимо указать текст вопроса")]
        public string TextQuetion { get; set; } = null!;
        public string? Option1 { get; set; } = null!;
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? Option5 { get; set; }


        [Required(ErrorMessage = "Необходимо указать правильный вариант ответа")]
        [Display(Name = "правильный ответ")]
        public string CorrectAnswer { get; set; } = null!;
        public int? WhoAnsweredQuestion { get; set; }
        public IFormFile? PictureTest { get; set; }

        //public virtual Test IdTestNavigation { get; set; } = null!;
        public virtual List<Answer> Answers { get; set; }
    }
}
