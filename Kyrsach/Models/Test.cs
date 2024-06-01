using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public partial class Test
    {
        public Test()
        {
            Questions = new List<Question>();
            Quezs = new List<Quez>();
        }
        [Key]
        public int IdTest { get; set; }
        public int IdCategory { get; set; }
        public string NameTest { get; set; } = null!;
        public virtual Category? IdCategoryNavigation { get; set; } = null!;
        public virtual List<Question>? Questions { get; set; }
        public virtual List<Quez>? Quezs { get; set; }
    }
}
