using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Framework.Topics
{
    public class Streaming
    {
        public static string GetCurrentDirectory() => AppDomain.CurrentDomain.BaseDirectory;

        public static string GetNetworkDirectory(string machineNameOrIPAddress) => $@"\\{machineNameOrIPAddress}\Shares";
        public static bool GetAccessControl(string filePath)
        {
            FileSecurity fileSecurity = new FileSecurity(); //File.GetAccessControl(filePath);
            
            fileSecurity.AddAccessRule(new FileSystemAccessRule(@"machineName\userName", FileSystemRights.FullControl, AccessControlType.Allow));
            
            //File.SetAccessControl(filePath, fileSecurity);
            FileStream fileStream = new FileStream(filePath, FileMode.Create, FileSystemRights.Write, FileShare.None, 8, FileOptions.Encrypted, fileSecurity);
            
            return true;
        }
    }
}
