using System;
using System.Diagnostics;
#if NET45 || NETFX_CORE
using System.Threading.Tasks;
#endif

namespace Pop3.IO
{
    internal interface INetworkOperations : IDisposable
    {
        #region Methods

#if !NETFX_CORE

        void Open( string hostName, int port );

        void Open( string hostName, int port, bool useSsl );
        
        string Read( );

        void Write( string data );
#endif

        void Close( );

        #endregion

        #region Async Methods

#if NET45 || NETFX_CORE
        Task OpenAsync( string hostName, int port );

        Task OpenAsync( string hostName, int port, bool useSsl );

        Task<string> ReadAsync( );

        Task WriteAsync( string data );
#endif

        #endregion
    }
}