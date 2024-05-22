using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public partial class Answer
    {

        public Answer(int v, int idQ, string answer, string loyal)
        {
            this.IdAnswersUser = v;
            this.IdQuestion = idQ;
            this.Answer1 = answer;
            this.Loyal = loyal;
        }
        public Answer() { }
        [Key]
        public int IdAnswer { get; set; }
        public int IdAnswersUser { get; set; }
        public int IdQuestion { get; set; }

        [Required(ErrorMessage = "Необходимо указать правильный вариант ответа")]
        [Display(Name = "правильный ответ")]
        public string Answer1 { get; set; } = null!;
        public string Loyal { get; set; } = null!;

        public virtual AnswersUser IdAnswersUserNavigation { get; set; } = null!;
        public virtual Question IdQuestionNavigation { get; set; } = null!;
    }
}
