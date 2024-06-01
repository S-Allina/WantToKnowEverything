using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public partial class Question
    {
        public Question()
        {
            Answers = new List<Answer>();
        }
        [Key]
        public int IdQuestion { get; set; }
        public int IdTest { get; set; }
        [Required(ErrorMessage = "Необходимо указать текст вопроса")]
        public string TextQuetion { get; set; } = null!;
        public string? Option1 { get; set; }
        public string? Option2 { get; set; }
        public string? Option3 { get; set; }
        public string? Option4 { get; set; }
        public string? Option5 { get; set; }
        public string CorrectAnswer { get; set; } = null!;
        public int? WhoAnsweredQuestion { get; set; }
        public byte[]? PictureTest { get; set; }

        public virtual Test IdTestNavigation { get; set; } = null!;
        public virtual List<Answer> Answers { get; set; }
    }
}
