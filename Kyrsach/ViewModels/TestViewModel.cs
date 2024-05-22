using Kyrsach.Models;

namespace Kyrsach.ViewModels
{
    public class TestViewModel
    {
        public TestViewModel()
        {
            Questions = new HashSet<Question>();
        }

        public int IdTest { get; set; }
        public int IdCategory { get; set; }
        public string NameTest { get; set; } = null!;
        public int? WhoAnsweredTest { get; set; }

        public virtual Category IdCategoryNavigation { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; }
    }
}
