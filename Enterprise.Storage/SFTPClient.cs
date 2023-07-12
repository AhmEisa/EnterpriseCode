using Renci.SshNet;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using static System.Net.WebRequestMethods;

namespace Enterprise.Storage
{
    public class SFTPClient
    {
        // you could pass the host, port, usr, pass, and uploadFile as parameters
        public void Upload()
        {
            var host = "10.1.55.87";
            var port = 22;
            var username = "inupco";
            var password = "sftp@2023";

            // path for file you want to upload
            var uploadFile = @"C:\wwwroot\e9bd279c-1651-4e72-80e5-c2805021376f.jpg";// 2ab8033f-3cc9-4cab-8db7-ed51516ec314.png";

            using (var client = new SftpClient(host, username, password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    Debug.WriteLine("I'm connected to the client");
                    client.ChangeDirectory(@"/home/inupco");
                    using (var fileStream = new FileStream(uploadFile, FileMode.Open))
                    {

                        client.BufferSize = 4 * 1024; // bypass Payload error large files
                        client.UploadFile(fileStream, Path.GetFileName($"/home/inupco/e9bd279c-1651-4e72-80e5-c2805021376f.jpg"));
                    }
                }
                else
                {
                    Debug.WriteLine("I couldn't connect");
                }
            }
        }
        public void Download()
        {
            var host = "10.1.55.87";
            var port = 22;
            var username = "inupco";
            var password = "sftp@2023";

            // path for file you want to download
            var downloadPath = @"C:\Users\ahmed\Documents\";
            //var fileName = @"ddf079fe-6a5a-4d64-86da-f5ee5e545fe5.png";
            string fileName = System.IO.Path.GetTempPath() + "ddf079fe-6a5a-4d64-86da-f5ee5e545fe5.png";
            using (var client = new SftpClient(host, username, password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    Debug.WriteLine("I'm connected to the client");
                    using (FileStream fs = new FileStream(Path.GetTempFileName(), FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.RandomAccess | FileOptions.DeleteOnClose))
                    {
                        client.ChangeDirectory(@"/home/inupco");
                        client.DownloadFile($"/home/inupco/ddf079fe-6a5a-4d64-86da-f5ee5e545fe5.png", fs);
                        fs.Seek(0, SeekOrigin.Begin);
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            fs.CopyTo(memoryStream);
                            string result = Convert.ToBase64String(memoryStream.ToArray());
                        }
                    }

                }
                else
                {
                    Debug.WriteLine("I couldn't connect");
                }
            }
        }
        public void Remove()
        {
            var host = "10.1.55.87";
            var port = 22;
            var username = "inupco";
            var password = "sftp@2023";
            using (var client = new SftpClient(host, username, password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    Debug.WriteLine("I'm connected to the client");

                    client.ChangeDirectory(@"/home/inupco");
                    client.DeleteFile($"/home/inupco/2ab8033f-3cc9-4cab-8db7-ed51516ec314.png");
                }
                else
                {
                    Debug.WriteLine("I couldn't connect");
                }
            }
        }
    }
}
