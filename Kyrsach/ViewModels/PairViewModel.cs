namespace Kyrsach.ViewModels
{
    public class PairViewModel
    {
        public int IdPair { get; set; }
        public int IdCategory { get; set; }
        public string? Card1Text { get; set; }
        public string? Card2Text { get; set; }
        public IFormFile? Card1Img { get; set; }
        public IFormFile? Card2Img { get; set; }

    }
}

