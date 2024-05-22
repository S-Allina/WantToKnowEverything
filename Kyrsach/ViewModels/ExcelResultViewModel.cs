namespace Kyrsach.ViewModels
{
    public class ExcelResultViewModel
    {
        public ExcelResultViewModel(string _NameTest, string _UserName, int _AverageScore) {
            NameTest = _NameTest;
            UserName = _UserName;
            AverageScore = _AverageScore;
        }
        public string NameTest { get; set; }
        public string UserName { get; set; }
        public int AverageScore { get; set; }
    }
}
