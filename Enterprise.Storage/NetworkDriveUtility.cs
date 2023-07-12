using System;
using System.Diagnostics;
using System.IO;

namespace Enterprise.Storage
{
    public class NetworkDriveUtility
    {
        static void Main(string[] args)
        {
            //bool success = new MountDrive.NetworkDriveUtility().MountNetworkDrive();
        }
        /// <summary>
        /// Mounts network drive on linux container
        /// </summary>
        /// <returns>Result</returns>
        public bool MountNetworkDrive()
        {

            bool result = false;

            try
            {
                //Replace these values with actual one
                string mountPath = "C:\\wwwroot"; // i.e. /LocalFolderName
                string sharePath = "\\\\10.1.51.121\\attachments";// "//<host>/<path>";
                string username = "attachment";
                string password = "Nupco@111";

                string mkdirArgs = $"-p \"{mountPath}\"";
                string mountArgs = $"-t cifs -o username={username},password={password} {sharePath} {mountPath}";

                string message = string.Empty;

                if (RunCommand("mkdir", mkdirArgs, out message))
                {
                    //Logger.LogInformation($"Output 1: {message}");

                    if (RunCommand("mount", mountArgs, out message))
                    {
                        //Logger.LogInformation($"Output 2: {message}");

                        string connectingTestingFile = "e9bd279c-1651-4e72-80e5-c2805021376f.jpg";// $"{Guid.NewGuid()}.txt";
                        string filePath = Path.Combine(mountPath, connectingTestingFile);

                        //Logger.LogInformation("Testing file path: " + filePath);

                        File.Create(filePath);
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                            result = true;
                        }
                        if (result)
                        {
                            //Logger.LogInformation("Network drive mounted successfully");
                        }
                        else
                        {
                            //Logger.LogError("Network drive mounting failed");
                        }
                    }
                    else
                    {
                        //Logger.LogError($"Error Output 2: {message}");
                    }
                }
                else
                {
                    //Logger.LogError($"Error Output 2: {message}");
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, $"Error message - {ex.Message}");
            }

            return result;
        }

        /// <summary>
        /// This method runs command on shell/bash
        /// </summary>
        /// <param name="command">Command name</param>
        /// <param name="args">Command argument</param>
        /// <param name="message">Output message</param>
        /// <returns>Boolean
        public static bool RunCommand(string command, string args, out string message)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = command,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (string.IsNullOrEmpty(error))
            {
                message = output;
                return true;
            }
            else
            {
                message = error;
                return true;
            }
        }
    }
}
