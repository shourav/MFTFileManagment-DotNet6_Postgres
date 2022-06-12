using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents.Data
{
    public class File
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Extension { get; set; } = string.Empty;
        [Required]
        public string Path { get; set; } = string.Empty;
        public string MakeBy { get; set; } = string.Empty;
        public DateTime MakeDate { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
