using AutoMapper;
using MFTFileManagment.ViewModels;
using Rebex.IO;
using Rebex.Net;

namespace MFTFileManagment
{
    public static class SFTP
    {
        public static async Task<List<string>> GetFileNamesfromServer(string license, string server, int port, string user, string password)
        {
            Rebex.Licensing.Key = license;
            List<string> result = new List<string>();
            using (var client = new Sftp())
            {
                await client.ConnectAsync(server, port);
                await client.LoginAsync(user, password);
                // retrieve and display the list of files and directories
                SftpItemCollection list = await client.GetListAsync();
                foreach (SftpItem item in list)
                {
                    result.Add(item.Name);
                }
                client.Disconnect();
            }
            return result;
        }

        public static async Task<List<string>> DownloadFileNamesfromServer(string makeBy, FileDataContext context, IMapper mapper, string license, string server, int port, string user, string password, string localPath)
        {
            //first get list of all files saved in database

            var dbRecords = await context.Files.OrderBy(j => j.Id).ToListAsync();
            var dbFileList = mapper.Map<List<FileViewModel>>(dbRecords);
            //setting Rebex License
            Rebex.Licensing.Key = license;
            //used to return saved file names
            List<string> result = new List<string>();
            //create connection
            using (var rebexClient = new Sftp())
            {
                await rebexClient.ConnectAsync(server, port);
                await rebexClient.LoginAsync(user, password);
                //get all files saved in SFTP Server
                SftpItemCollection list = await rebexClient.GetListAsync();
                //iterate over files
                foreach (SftpItem item in list)
                {
                    //if file is not already in database: based on name (case insensitive) & creation time
                    //save the file in local folder and save in db)
                    if (item.IsFile
                        && ((
                            (dbFileList != null && dbFileList.Count > 0)
                            && !dbFileList.Exists
                                (f =>
                                    (f.Name.ToLower() == item.Name.ToLower()
                                    && (f.CreationTime != null && item.CreationTime != null &&
                                    f.CreationTime.Value.ToUniversalTime() == item.CreationTime.Value.ToUniversalTime()
                                        )
                                    )
                                )
                            ) || (dbFileList == null || dbFileList.Count == 0)) //in case no file present in db save all files from server
                        )
                    {
                        await rebexClient.DownloadAsync(item.Name
                                                , localPath
                                                , TraversalMode.MatchFilesShallow
                                                , TransferMethod.Copy
                                                , ActionOnExistingFiles.OverwriteAll
                                            );
                        context.Files.Add(new Documents.Data.File
                        {
                            Name = item.Name,
                            Path = localPath + item.Name,
                            Extension = Path.GetExtension(item.Name),
                            MakeBy = makeBy,
                            MakeDate = DateTime.UtcNow,
                            CreationTime = item.CreationTime.HasValue ? item.CreationTime.Value.ToUniversalTime() : (DateTime?)null,
                            Remarks = String.Empty
                        });
                        await context.SaveChangesAsync();
                        result.Add(item.Name);
                    }
                }
                rebexClient.Disconnect();
            }
            //return a the names of files saved in this session
            return result;
        }
    }
}