namespace MFTFileManagment
{
    public class Document
    {
        public long DocumentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Extension { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string MakeBy { get; set; } = string.Empty;
        public DateTime MakeDate { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}