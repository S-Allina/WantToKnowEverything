using Kyrsach.Models;

namespace Kyrsach.ViewModels
{
    public class QuezViewModel
    {
        public int IdTest { get; set; }
        public int IdQuez { get; set; }
        public string Q1 { get; set; } = null!;
        public string Q2 { get; set; } = null!;
        public string Q3 { get; set; } = null!;
        public string Q4 { get; set; } = null!;
        public string Q5 { get; set; } = null!;
        public IFormFile? Pic1 { get; set; }
        public IFormFile? Pic2 { get; set; }
        public IFormFile? Pic3 { get; set; }
        public IFormFile? Pic4 { get; set; }
        public IFormFile? Pic5 { get; set; }
        public virtual Test IdTestNavigation { get; set; } = null!;


        //public virtual Quez IdQuezNavigation { get; set; } = null!;

    }
}
