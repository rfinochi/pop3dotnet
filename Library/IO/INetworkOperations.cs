using System;
using System.Diagnostics;
#if NET45
using System.Threading.Tasks;
#endif

namespace Pop3.IO
{
    public interface INetworkOperations : IDisposable
    {
        #region Methods

        void Open( string hostName, int port );

        void Open( string hostName, int port, bool useSsl );
        
        string Read( );

        void Write( string data );

        void Close( );

        #endregion

        #region Async Methods

#if NET45  
        Task OpenAsync( string hostName, int port );

        Task OpenAsync( string hostName, int port, bool useSsl );

        Task<string> ReadAsync( );

        Task WriteAsync( string data );
#endif

        #endregion
    }
}