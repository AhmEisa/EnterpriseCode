namespace Enterprise.Storage
{
    public class AttachmentConfiguration
    {
        #region Props
        /// <summary>
        /// UploadUrl
        /// </summary>
        public string UploadUrl { get; set; }

        /// <summary>
        ///Shared Folder Path
        /// </summary>
        public string SharedFolderPath { get; set; }

        /// <summary>
        ///Username can edit Network Shared Path
        /// </summary>
        public string SharedFolderUserName { get; set; }

        /// <summary>
        /// Secret of user can edit Network Shared Path
        /// </summary>
        public string SharedFolderSecret { get; set; }

        /// <summary>
        /// MaxFileSizeInBytes
        /// </summary>
        public long MaxFileSizeInBytes { get; set; }

        /// <summary>
        /// AllowedFileExtensions
        /// </summary>
        public string[] AllowedFileExtensions { get; set; }

        #endregion

    }
}
