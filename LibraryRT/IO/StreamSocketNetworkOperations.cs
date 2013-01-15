/*
 * Author: Rodolfo Finochietti
 * Email: rfinochi@shockbyte.net
 * Web: http://shockbyte.net
 *
 * This work is licensed under the Creative Commons Attribution License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by/2.0
 * or send a letter to Creative Commons, 559 Nathan Abbott Way, Stanford, California 94305, USA.
 * 
 * No warranties expressed or implied, use at your own risk.
 */
using System;
using System.Text;
using System.Threading.Tasks;

using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;

namespace Pop3.IO
{
    internal class StreamSocketNetworkOperations : INetworkOperations
    {
        #region Private Fields

        private StreamSocket _socket;

        #endregion

        #region INetworkOperations

        #region Public Methods

        public void Close( )
        {
            if ( _socket != null )
            {
                _socket.Dispose( );
                _socket = null;
            }                  
        }

        #endregion
        
        #region Public Async Methods

        public async Task OpenAsync( string hostName, int port )
        {
            await OpenAsync( hostName, port, false ).ConfigureAwait( false );
        }

        public async Task OpenAsync( string hostName, int port, bool useSsl )
        {
            if ( _socket == null )
            {
                _socket = new StreamSocket( );

                if ( useSsl )
                    await _socket.ConnectAsync( new HostName( hostName ), port.ToString( ), SocketProtectionLevel.Ssl );
                else
                    await _socket.ConnectAsync( new HostName( hostName ), port.ToString( ) );
            }
        }
        
        public async Task<string> ReadAsync( )
        {
            if ( _socket == null )
                throw new InvalidOperationException( "The Network Socket is null" );

            UTF8Encoding enc = new UTF8Encoding( );
            byte[] serverBuffer = new Byte[ 1024 ];
            int count = 0;

            while ( true )
            {
                using ( DataReader reader = new DataReader( _socket.InputStream ) )
                {
                    uint bytes = await reader.LoadAsync( 1 );
                    if ( bytes != 1 )
                        break;

                    serverBuffer[ count ] = reader.ReadByte( );
                    count++;

                    if ( serverBuffer[ count ] == '\n' )
                        break;
                }
            }

            return enc.GetString( serverBuffer, 0, count );
        }

        public async Task WriteAsync( string data )
        {
            if ( _socket == null )
                throw new InvalidOperationException( "Pop3 client already connected" );

            using ( DataWriter writer = new DataWriter( _socket.OutputStream ) )
            {
                writer.WriteString( data );
                await writer.StoreAsync( );
            }
        }

        #endregion

        #endregion

        #region Dispose-Finalize Pattern

        public void Dispose( )
        {
            Close( );

            GC.SuppressFinalize( this );
        }

        #endregion
    }
}