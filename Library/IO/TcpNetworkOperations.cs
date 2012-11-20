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
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
#if NET45
using System.Threading.Tasks;
#endif

namespace Pop3.IO
{
    internal class TcpNetworkOperations : INetworkOperations
    {
        #region Private Fields

        private TcpClient _tcpClient;
        private Stream _stream;

        #endregion

        #region INetworkOperations

        #region Public Methods

        public void Open( string hostName, int port )
        {
            Open( hostName, port, false );
        }
        
        public void Open( string hostName, int port, bool useSsl )
        {
            if ( _tcpClient == null )
            {
                _tcpClient = new TcpClient( );
                _tcpClient.Connect( hostName, port );

                if ( useSsl )
                {
                    _stream = new SslStream( _tcpClient.GetStream( ), false );
                    ( (SslStream)_stream ).AuthenticateAsClient( hostName );
                }
                else
                {
                    _stream = _tcpClient.GetStream( );
                }
            }
        }
        
        public string Read( )
        {
            if ( _stream == null )
                throw new Pop3Exception( "The Network Stream is null" );

            ASCIIEncoding enc = new ASCIIEncoding( );
            byte[] serverBuffer = new Byte[ 1024 ];
            int count = 0;

            while ( true )
            {
                byte[] buff = new Byte[ 2 ];
                int bytes = _stream.Read( buff, 0, 1 );
                if ( bytes != 1 )
                    break;

                serverBuffer[ count ] = buff[ 0 ];
                count++;

                if ( buff[ 0 ] == '\n' )
                    break;
            }

            return enc.GetString( serverBuffer, 0, count );
        }

        public void Write( string data )
        {
            if ( _stream == null )
                throw new Pop3Exception( "The Network Stream is null" );

            ASCIIEncoding en = new ASCIIEncoding( );

            byte[] writeBuffer = en.GetBytes( data );

            _stream.Write( writeBuffer, 0, writeBuffer.Length );
        }
        
        public void Close( )
        {
            IDisposable disposableStream = _stream as IDisposable;
            if ( disposableStream != null )
                disposableStream.Dispose( );

            if ( _tcpClient != null )
            {
                _tcpClient.Close( );
                _tcpClient = null;
            }                  
        }

        #endregion

        #region Public Async Methods

#if NET45
        public async Task OpenAsync( string hostName, int port )
        {
            await OpenAsync( hostName, port, false );
        }

        public async Task OpenAsync( string hostName, int port, bool useSsl )
        {
            if ( _tcpClient == null )
            {
                _tcpClient = new TcpClient( );
                await _tcpClient.ConnectAsync( hostName, port );

                if ( useSsl )
                {
                    _stream = new SslStream( _tcpClient.GetStream( ), false );
                    ( (SslStream)_stream ).AuthenticateAsClient( hostName );
                }
                else
                {
                    _stream = _tcpClient.GetStream( );
                }
            }
        }
        
        public async Task<string> ReadAsync( )
        {
            if ( _stream == null )
                throw new Pop3Exception( "The Network Stream is null" );

            ASCIIEncoding enc = new ASCIIEncoding( );
            byte[] serverBuffer = new Byte[ 1024 ];
            int count = 0;

            while ( true )
            {
                byte[] buff = new Byte[ 2 ];
                int bytes = await _stream.ReadAsync( buff, 0, 1 );
                if ( bytes != 1 )
                    break;

                serverBuffer[ count ] = buff[ 0 ];
                count++;

                if ( buff[ 0 ] == '\n' )
                    break;
            }

            return enc.GetString( serverBuffer, 0, count );
        }

        public async Task WriteAsync( string data )
        {
            if ( _stream == null )
                throw new Pop3Exception( "The Network Stream is null" );

            ASCIIEncoding en = new ASCIIEncoding( );

            byte[] writeBuffer = en.GetBytes( data );

            await _stream.WriteAsync( writeBuffer, 0, writeBuffer.Length );
        }
#endif

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