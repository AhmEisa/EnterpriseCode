using System;
using System.IO;
using System.Threading.Tasks;

namespace Enterprise.Storage
{
    public class FileSystemStorageService
    {
        #region Props
        private readonly AttachmentConfiguration _attachmentConfiguration;

        #endregion

        #region Ctor
        /// <summary>
        /// FileSystemStorageService
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public FileSystemStorageService(AttachmentConfiguration options)
        {
            _attachmentConfiguration = options;
        }

        #endregion

        #region Behavior

        /// <summary>
        /// Upload Attachment file in File System
        /// </summary>
        /// <param name="uploadFileDto"></param>
        /// <returns></returns>
        public async Task<string> UploadFile(StorageUploadFileDto uploadFileDto)
        {
            try
            {
                //Create a folder if not exist
                if (!Directory.Exists(_attachmentConfiguration.UploadUrl))
                {
                    Directory.CreateDirectory(_attachmentConfiguration.UploadUrl);
                }

                //TODO: check security antivirus 
                //AntivirusCheckCommand check google check virus, (cloud service will check the file and get the response to user) 
                // may use statusId as => Virus Scan => depend on the cloud service

                // upload file to Server
                var attchmentStorageId = $"{uploadFileDto.AttachmentId}{uploadFileDto.FileExtension}";
                string filePath = Path.Combine(_attachmentConfiguration.UploadUrl, attchmentStorageId);

                using (Stream stream = new FileStream(filePath, FileMode.Create))
                {
                    //await stream.WriteAsync(Convert.FromBase64String(uploadFileDto.Content));
                }

                return attchmentStorageId;
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, ex.Message);
                return String.Empty;
            }
        }

        /// <summary>
        /// Donwload file from file system
        /// </summary>
        /// <param name="downloadFileDto"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<string> DownloadFile(StorageDownloadFileDto downloadFileDto)
        {
            try
            {
                string filePath = Path.Combine(_attachmentConfiguration.UploadUrl, downloadFileDto.DocumentStorageId);

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
        /// Delete Attachment file in File System
        /// </summary>
        /// <param name="deleteFileDto"></param>
        /// <returns></returns>
        public bool DeleteFile(StorageDeleteFileDto deleteFileDto)
        {
            try
            {
                if (deleteFileDto == null || string.IsNullOrWhiteSpace(deleteFileDto.DocumentStorageId)) return true;

                string filePath = Path.Combine(_attachmentConfiguration.UploadUrl, deleteFileDto.DocumentStorageId);

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
    }
}
