using System.ComponentModel.DataAnnotations;

namespace Kyrsach.Models
{
    public partial class Quez
    {
        public int IdTest { get; set; }
        [Key]
        public int IdQuez { get; set; }
        public string Q1 { get; set; } = null!;
        public string Q2 { get; set; } = null!;
        public string Q3 { get; set; } = null!;
        public string Q4 { get; set; } = null!;
        public string Q5 { get; set; } = null!;
        public byte[]? Pic1 { get; set; }
        public byte[]? Pic2 { get; set; }
        public byte[]? Pic3 { get; set; }
        public byte[]? Pic4 { get; set; }
        public byte[]? Pic5 { get; set; }

        public virtual Test IdTestNavigation { get; set; } = null!;
    }
}
