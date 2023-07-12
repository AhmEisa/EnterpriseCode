using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;

namespace Enterprise.Storage
{
    #region Helper
    /// <summary>
    /// ConnectToSharedFolder
    /// </summary>
    public class ConnectToSharedFolder : IDisposable
    {
        #region Props
        readonly string _networkName;
        #endregion

        #region Ctor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="networkName"></param>
        /// <param name="credentials"></param>
        /// <exception cref="Win32Exception"></exception>
        public ConnectToSharedFolder(string networkName, NetworkCredential credentials)
        {
            _networkName = networkName;

            var netResource = new NetResource
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = networkName
            };

            var userName = string.IsNullOrEmpty(credentials.Domain)
                ? credentials.UserName
                : string.Format(@"{0}\{1}", credentials.Domain, credentials.UserName);

            var result = WNetAddConnection2(
                netResource,
                credentials.Password,
                userName,
                0);

            if (result != 0)
            {
                throw new Win32Exception(result, "Error connecting to remote share");
            }
        }

        /// <summary>
        /// ConnectToSharedFolder
        /// </summary>
        ~ConnectToSharedFolder()
        {
            Dispose(false);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_networkName, 0, true);
        }

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource, string password, string username, int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags, bool force);
        #endregion

        #region Helpers

        /// <summary>
        /// NetResource
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public class NetResource
        {
            public ResourceScope Scope;
            public ResourceType ResourceType;
            public ResourceDisplaytype DisplayType;
            public int Usage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        /// <summary>
        /// ResourceScope
        /// </summary>
        public enum ResourceScope : int
        {
            Connected = 1,
            GlobalNetwork,
            Remembered,
            Recent,
            Context
        };

        /// <summary>
        /// ResourceType
        /// </summary>
        public enum ResourceType : int
        {
            Any = 0,
            Disk = 1,
            Print = 2,
            Reserved = 8,
        }

        /// <summary>
        /// ResourceDisplaytype
        /// </summary>
        public enum ResourceDisplaytype : int
        {
            Generic = 0x0,
            Domain = 0x01,
            Server = 0x02,
            Share = 0x03,
            File = 0x04,
            Group = 0x05,
            Network = 0x06,
            Root = 0x07,
            Shareadmin = 0x08,
            Directory = 0x09,
            Tree = 0x0a,
            Ndscontainer = 0x0b
        }
        #endregion
    }
    #endregion
}
