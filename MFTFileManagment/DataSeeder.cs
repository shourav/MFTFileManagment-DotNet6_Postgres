using Documents.Data;

namespace MFTFileManagment
{
    public static class DataSeeder
    {
        public static void Seed(this IHost host)
        {
            using var scope = host.Services.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<FileDataContext>();
            context.Database.EnsureCreated();
            AddFiles(context);
        }

        private static void AddFiles(FileDataContext context)
        {
            var file = context.Files.FirstOrDefault();
            if (file != null) return;
            context.Files.Add(new Documents.Data.File
            {
                Name="Test100.pdf",
                Path= "C:\\Users\\shourav.banik\\source\\repos\\MFTFileManagment\\MFTFileManagment\\Attachments\\Test100.pdf",
                Extension=".pdf",
                MakeBy="sbanik",
                MakeDate= DateTime.UtcNow,
                Remarks ="Default Data"

            });
            context.Files.Add(new Documents.Data.File
            {
                Name = "Test101.pdf",
                Path = "C:\\Users\\shourav.banik\\source\\repos\\MFTFileManagment\\MFTFileManagment\\Attachments\\Test101.pdf",
                Extension = ".pdf",
                MakeBy = "sbanik",
                MakeDate = DateTime.UtcNow,
                Remarks = "Default Data"

            });
            context.Files.Add(new Documents.Data.File
            {
                Name = "Test102.pdf",
                Path = "C:\\Users\\shourav.banik\\source\\repos\\MFTFileManagment\\MFTFileManagment\\Attachments\\Test102.pdf",
                Extension = ".pdf",
                MakeBy = "sbanik",
                MakeDate = DateTime.UtcNow,
                Remarks = "Default Data"

            });
            context.Files.Add(new Documents.Data.File
            {
                Name = "Test103.pdf",
                Path = "C:\\Users\\shourav.banik\\source\\repos\\MFTFileManagment\\MFTFileManagment\\Attachments\\Test103.pdf",
                Extension = ".pdf",
                MakeBy = "sbanik",
                MakeDate = DateTime.UtcNow,
                Remarks = "Default Data"

            });
            context.SaveChanges();
        }
    }
}
