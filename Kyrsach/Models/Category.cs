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
        public string NameCategory { get; set; } = null!;
        public string WhoCreatedCategory { get; set; }
        public byte[]? Picture { get; set; }
        public string? Type { get; set; }

        public virtual List<Pair> Pairs { get; set; }
        public virtual List<Test> Tests { get; set; }
    }
}
