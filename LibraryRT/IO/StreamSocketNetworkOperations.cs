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
                _writer = new DataWriter( _socket.OutputStream );
            }
        }

        public async Task<string> ReadAsync( )
        {
            if ( _socket == null )
                throw new InvalidOperationException( "The Network Socket is null" );

            byte[] buffer = new byte[ Constants.BufferSize ];
            int count = 0;

            while ( true )
            {
                uint bytes = await _reader.LoadAsync( 1 );
                if ( bytes != 1 )
                    break;

                buffer[ count ] = _reader.ReadByte( );
                count++;

                if ( count >= Constants.BufferSize )
                    throw new OutOfMemoryException( String.Format( CultureInfo.InvariantCulture, "The message is to large (current buffer size {0})", Constants.BufferSize ) );

                if ( buffer[ count ] == '\n' )
                    break;
            }

            UTF8Encoding enc = new UTF8Encoding( );
            return enc.GetString( buffer, 0, count );
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