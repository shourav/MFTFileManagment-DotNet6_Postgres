using Rebex.IO;
using Rebex.Net;
namespace MFTFileManagment
{
    public static class SFTP
    {
        public static List<string> GetFileNamesfromServer(string license, string server, int port, string user, string password)
        {
            Rebex.Licensing.Key = license;
            List<string> result = new List<string>();
            Sftp client = new Sftp();
            client.Connect(server, port);
            client.Login(user, password);

            // select the desired directory
            //client.ChangeDirectory("path");

            // retrieve and display the list of files and directories
            SftpItemCollection list = client.GetList();
            foreach (SftpItem item in list)
            {
                result.Add(item.Name);
                //Console.Write(item.LastWriteTime.Value.ToString("u"));
                //Console.Write(item.Length.ToString().PadLeft(10, ' '));
                //Console.Write(" {0}", item.Name);
                //Console.WriteLine();
            }
            client.Disconnect();
            return result;
        }

        public static List<string> DownloadFileNamesfromServer(string license, string server, int port, string user, string password, string localPath)
        {
            Rebex.Licensing.Key = license;
            List<string> result = new List<string>();
            Sftp client = new Sftp();
            client.Connect(server, port);
            client.Login(user, password);

            // select the desired directory
            //client.ChangeDirectory("path");

            client.Download("/*"
                            , localPath
                            , TraversalMode.MatchFilesShallow
                            , TransferMethod.Copy
                            , ActionOnExistingFiles.OverwriteAll
                            );

            client.Disconnect();
            return result;
        }
    }
}
