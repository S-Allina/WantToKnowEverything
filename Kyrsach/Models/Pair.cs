using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public partial class Pair
    {
        [Key]
        public int IdPair { get; set; }
        public int IdCategory { get; set; }
        public string? Card1Text { get; set; }
        public string? Card2Text { get; set; }
        public byte[]? Card1Img { get; set; }
        public byte[]? Card2Img { get; set; }

        public virtual Category? IdCategoryNavigation { get; set; } 
    }
}
