using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public partial class AnswersUser
    {
        public AnswersUser()
        {
            Answers = new List<Answer>();
        }



        public AnswersUser(string idUse, int CoutC, DateTime time)
        {
            this.Time = time;
            this.CountCurrent = CoutC;
            this.IdUser = idUse;
        }
        [Key]
        public int IdAnswersUser { get; set; }
        public string IdUser { get; set; }
        public int CountCurrent { get; set; }
        public DateTime? Time { get; set; }

        public virtual List<Answer> Answers { get; set; }
    }
}
