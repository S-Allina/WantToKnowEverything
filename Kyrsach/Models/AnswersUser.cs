using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public partial class AnswersUser
    {
      
        [Key]
        public int IdAnswersUser { get; set; }
        public string IdUser { get; set; }
        public int CountCurrent { get; set; }
        public DateTime? Time { get; set; }

        public virtual List<Answer> Answers { get; set; }
    }
}
