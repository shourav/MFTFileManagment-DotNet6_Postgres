namespace MFTFileManagment.ViewModels
{
    public class FileViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }   
        public string Path { get; set; }   
        public string Extension { get; set; }   
        public string MakeBy { get; set; }   
        public DateTime MakeDate { get; set; }
        public string Remarks { get; set; }

    }
}
