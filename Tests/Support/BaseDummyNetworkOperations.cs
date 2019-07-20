/*
 * Author: Rodolfo Finochietti
 * Email: rfinochi@shockbyte.net
 * Web: http://shockbyte.net
 *
 * This work is licensed under the MIT License. 
 * 
 * No warranties expressed or implied, use at your own risk.
 */
using System.Threading.Tasks;

using Pop3.IO;

namespace Pop3.Tests.Support
{
    public abstract class BaseDummyNetworkOperations : INetworkOperations
    {
        #region INetworkOperations

        #region Public Methods

        public void Open( string hostName, int port )
        {
        }

        public void Open( string hostName, int port, bool useSsl )
        {
        }

        public void Open( string hostName, int port, bool useSsl, bool checkCertificate )
        {
        }

        public string Read( )
        {
            return GetData( );
        }

        public void Write( string data )
        {
        }

        public void Close( )
        {
        }

        #endregion

        #region Public Async Methods

        public async Task OpenAsync( string hostName, int port )
        {
            await OpenAsync( hostName, port, false ).ConfigureAwait( false );
        }

        public async Task OpenAsync( string hostName, int port, bool useSsl )
        {
            await Task.Delay( 1 ).ConfigureAwait( false );
        }

        public async Task<string> ReadAsync( )
        {
            return await Task.Run( async ( ) =>
                                    {
                                        await Task.Delay( 0 );
                                        return GetData( );
                                    } ).ConfigureAwait( false );
        }

        public async Task WriteAsync( string data )
        {
            await Task.Delay( 0 ).ConfigureAwait( false );
        }

        #endregion

        #region Dispose-Finalize Pattern

        public void Dispose( )
        {
        }

        #endregion

        #endregion

        #region Private Methods

        protected abstract string GetData( );

        #endregion
    }
}