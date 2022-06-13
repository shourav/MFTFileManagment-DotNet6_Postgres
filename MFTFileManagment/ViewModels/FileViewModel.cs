using System.Text;

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
        public DateTime? CreationTime { get; set; }
        public string Remarks { get; set; }

        public override string ToString()
        {
            return GetType().GetProperties()
                .Select(info => (info.Name, Value: info.GetValue(this, null) ?? "(null)"))
                .Aggregate(
                    new StringBuilder(),
                    (sb, pair) => sb.AppendLine($"{pair.Name}: {pair.Value}"),
                    sb => sb.ToString());
        }

    }
}
