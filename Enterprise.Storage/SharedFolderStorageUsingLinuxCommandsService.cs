using Renci.SshNet;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Enterprise.Storage
{
    /// <summary>
    /// Shared Folder Storage Service
    /// </summary>
    public class SharedFolderStorageUsingLinuxCommandsService 
    {
        #region Props
        private readonly AttachmentConfiguration _attachmentConfiguration;
       // private readonly ILogger<SharedFolderStorageUsingLinuxCommandsService> _logger;
        private readonly NetworkCredential _credentials;
        #endregion

        #region Ctor
        /// <summary>
        /// SharedFolderStorageService
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public SharedFolderStorageUsingLinuxCommandsService(AttachmentConfiguration options)
        {
            _attachmentConfiguration = options;
            _credentials = new NetworkCredential(_attachmentConfiguration.SharedFolderUserName, _attachmentConfiguration.SharedFolderSecret);
        }

        #endregion

        #region Behavior

        /// <summary>
        /// Upload Attachment file in Shared Folder
        /// </summary>
        /// <param name="uploadFileDto"></param>
        /// <returns></returns>
        public async Task<string> UploadFile(StorageUploadFileDto uploadFileDto)
        {
            try
            {
                SftpClient sftpclient = new SftpClient("hostname", "username", "pwd");
                sftpclient.Connect();

                string localFilename = "";// FUupload.PostedFile.FileName;
                string remoteFilename = "/wwabc1/test/folder_one/test/" + Path.GetFileName("filename");
                using (var fileStream = File.OpenRead(localFilename))
                {
                    sftpclient.UploadFile(fileStream, remoteFilename);
                }

                string mountPath = "/var/shared"; // i.e. /LocalFolderName
                string mkdirArgs = $"-p \"{mountPath}\"";
                string mountArgs = $"-t cifs -o username={_attachmentConfiguration.SharedFolderUserName},password={_attachmentConfiguration.SharedFolderSecret} {_attachmentConfiguration.SharedFolderPath} {mountPath}";
                string message = string.Empty;
                if (!RunCommand("mkdir", mkdirArgs, out message)) ;// { _logger.LogError($"Error mkdir : {message}"); return string.Empty; }
                if (!RunCommand("mount", mountArgs, out message)) ;// { _logger.LogError($"Error mount : {message}"); return string.Empty; }

                //Create a folder if not exist
                if (!Directory.Exists(mountPath))
                {
                    Directory.CreateDirectory(mountPath);
                }

                //TODO: check security antivirus 
                //AntivirusCheckCommand check google check virus, (cloud service will check the file and get the response to user) 
                // may use statusId as => Virus Scan => depend on the cloud service

                // upload file to Server
                var attchmentStorageId = $"{uploadFileDto.AttachmentId}{uploadFileDto.FileExtension}";
                string filePath = Path.Combine(mountPath, attchmentStorageId);

                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                   // await stream.WriteAsync(Convert.FromBase64String(uploadFileDto.Content));
                }

                return attchmentStorageId;

            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// Donwload file from Shared Folder
        /// </summary>
        /// <param name="downloadFileDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> DownloadFile(StorageDownloadFileDto downloadFileDto)
        {
            try
            {
                string mountPath = "/tmp/SharedFolder"; // i.e. /LocalFolderName
                string mkdirArgs = $"-p \"{mountPath}\"";
                string mountArgs = $"-t cifs -o username={_attachmentConfiguration.SharedFolderUserName},password={_attachmentConfiguration.SharedFolderSecret} {_attachmentConfiguration.SharedFolderPath} {mountPath}";
                string message = string.Empty;
                if (!RunCommand("mkdir", mkdirArgs, out message)) ;// { _logger.LogError($"Error mkdir : {message}"); return string.Empty; }
                if (!RunCommand("mount", mountArgs, out message)) ;// { _logger.LogError($"Error mount : {message}"); return string.Empty; }

                string filePath = Path.Combine(mountPath, downloadFileDto.DocumentStorageId);

                //Check if file not exist
                if (!File.Exists(filePath))
                { return string.Empty; }

                using (FileStream inputFile = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None, bufferSize: 1024 * 1024, useAsync: true)) // When using `useAsync: true` you get better performance with buffers much larger than the default 4096 bytes.
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    await inputFile.CopyToAsync(memoryStream).ConfigureAwait(false);
                    return Convert.ToBase64String(memoryStream.ToArray());
                }

            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// Delete Attachment file in Shared Folder
        /// </summary>
        /// <param name="deleteFileDto"></param>
        /// <returns></returns>
        public bool DeleteFile(StorageDeleteFileDto deleteFileDto)
        {
            try
            {
                if (deleteFileDto == null || string.IsNullOrWhiteSpace(deleteFileDto.DocumentStorageId)) return true;

                string mountPath = "/tmp/SharedFolder"; // i.e. /LocalFolderName
                string mkdirArgs = $"-p \"{mountPath}\"";
                string mountArgs = $"-t cifs -o username={_attachmentConfiguration.SharedFolderUserName},password={_attachmentConfiguration.SharedFolderSecret} {_attachmentConfiguration.SharedFolderPath} {mountPath}";
                string message = string.Empty;
                if (!RunCommand("mkdir", mkdirArgs, out message)) ;// { _logger.LogError($"Error mkdir : {message}"); return false; }
                if (!RunCommand("mount", mountArgs, out message)) ;//{ _logger.LogError($"Error mount : {message}"); return false; }

                string filePath = Path.Combine(mountPath, deleteFileDto.DocumentStorageId);

                //Check if file not exist
                if (!File.Exists(filePath))
                { return true; }

                File.Delete(filePath);

                //_logger.LogInformation($"Attachment with storage Id {deleteFileDto.DocumentStorageId} deleted from storage service!");

                return true;

            }
            catch (Exception ex)
            {
               // _logger.LogError(ex, ex.Message);
                return false;
            }
        }


        #endregion

        #region Helpers
        /// <summary>
        /// This method runs command on shell/bash
        /// </summary>
        /// <param name="command">Command name</param>
        /// <param name="args">Command argument</param>
        /// <param name="message">Output message</param>
        /// <returns>Boolean
        private static bool RunCommand(string command, string args, out string message)
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
                return false;
            }
        }
        #endregion
    }
}
