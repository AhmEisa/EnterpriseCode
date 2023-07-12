using System;

namespace Enterprise.Storage
{
    public class StorageUploadFileDto
    {
        #region Props.

        /// <summary>
        /// AttachmentId
        /// </summary>
        public Guid AttachmentId { get; set; }

        /// <summary>
        /// Document Storage Id
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// File Extension
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Document Type 
        /// </summary>
        public string DocumentType { get; set; }

        #endregion
    }
}
