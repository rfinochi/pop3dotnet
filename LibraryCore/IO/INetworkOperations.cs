using System;
using System.Threading.Tasks;

namespace Pop3.IO
{
    internal interface INetworkOperations : IDisposable
    {
        #region Methods

#if FULL
        void Open( string hostName, int port );

        void Open( string hostName, int port, bool useSsl );
        
        string Read( );

        void Write( string data );
#endif

        void Close( );

        #endregion

    #region Async Methods

#if !NET40
        Task OpenAsync( string hostName, int port );

        Task OpenAsync( string hostName, int port, bool useSsl );

        Task<string> ReadAsync( );

        Task WriteAsync( string data );
#endif

        #endregion
    }
}