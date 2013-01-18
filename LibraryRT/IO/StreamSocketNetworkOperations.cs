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
using System.Globalization;
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
        private DataReader _reader;
        private DataWriter _writer;

        #endregion

        #region INetworkOperations

        #region Public Methods

        public void Close( )
        {
            if ( _reader != null )
            {
                _reader.DetachStream( );
                _reader.Dispose( );
                _reader = null;
            }

            if ( _writer != null )
            {
                _writer.DetachStream( );
                _writer.Dispose( );
                _writer = null;
            }
            
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

                _reader = new DataReader( _socket.InputStream );
                _reader.InputStreamOptions = InputStreamOptions.Partial;

                _writer = new DataWriter( _socket.OutputStream );
            }
        }

        public async Task<string> ReadAsync( )
        {
            if ( _socket == null )
                throw new InvalidOperationException( "The Network Socket is null" );

            StringBuilder sb = new StringBuilder();

            while ( true )
            {
                uint dataLength = await _reader.LoadAsync( 1 );
                if ( dataLength != 1 )
                    break;

                sb.Append( _reader.ReadString( _reader.UnconsumedBufferLength ) );

                if ( sb[ sb.Length - 1 ] == '\n' )
                    break;
            }

            return sb.ToString( );
        }

        public async Task WriteAsync( string data )
        {
            if ( _socket == null )
                throw new InvalidOperationException( "Pop3 client already connected" );

            _writer.WriteString( data );
            await _writer.StoreAsync( );
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