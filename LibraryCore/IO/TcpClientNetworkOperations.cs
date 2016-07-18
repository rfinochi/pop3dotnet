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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Pop3.IO
{
    internal class TcpClientNetworkOperations : INetworkOperations
    {
        #region Private Fields

        private TcpClient _tcpClient;
        private Stream _stream;
        private static bool _checkCertificate;

        #endregion

        #region INetworkOperations

        #region Public Methods

#if FULL
        public void Open( string hostName, int port )
        {
            Open( hostName, port, false, true );
        }

        public void Open( string hostName, int port, bool useSsl )
        {
            Open( hostName, port, useSsl, true );
        }

        public void Open( string hostName, int port, bool useSsl, bool checkCertificate )
        {
            _checkCertificate = checkCertificate;

            if ( _tcpClient == null )
            {
                _tcpClient = new TcpClient( );
                _tcpClient.Connect( hostName, port );

                if ( useSsl )
                {
                    _stream = new SslStream( _tcpClient.GetStream( ), false, ValidateServerCertificate );
                    ( (SslStream)_stream ).AuthenticateAsClient( hostName );
                }
                else
                {
                    _stream = _tcpClient.GetStream( );
                }
            }
        }

        private static bool ValidateServerCertificate( object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors )
        {
            if ( !_checkCertificate )
            {
                return true;
            }
            if ( sslPolicyErrors == SslPolicyErrors.None )
            {
                return true;
            }
            return false;
        }

        public string Read( )
        {
            if ( _stream == null )
                throw new InvalidOperationException( "The Network Stream is null" );

            byte[] data = new byte[ 1 ];
            StringBuilder sb = new StringBuilder( );
            UTF8Encoding enc = new UTF8Encoding( );

            while ( true )
            {
                int dataLength = _stream.Read( data, 0, 1 );
                if ( dataLength != 1 )
                    break;

                sb.Append( enc.GetString( data, 0, 1 ) );

                if ( data[ 0 ] == '\n' )
                    break;
            }

            return sb.ToString( );
        }

        public void Write( string data )
        {
            if ( _stream == null )
                throw new InvalidOperationException( "The Network Stream is null" );

            UTF8Encoding en = new UTF8Encoding( );
            byte[] writeBuffer = en.GetBytes( data );

            _stream.Write( writeBuffer, 0, writeBuffer.Length );
        }
#endif

        public void Close( )
        {
            IDisposable disposableStream = _stream as IDisposable;
            if ( disposableStream != null )
                disposableStream.Dispose( );

            if ( _tcpClient != null )
            {
#if FULL
                _tcpClient.Close( );
#endif
#if !NET40
                _tcpClient.Dispose( );
#endif
                _tcpClient = null;
            }
        }

        #endregion

        #region Public Async Methods

#if !NET40
        public async Task OpenAsync( string hostName, int port )
        {
            await OpenAsync( hostName, port, false ).ConfigureAwait( false );
        }

        public async Task OpenAsync( string hostName, int port, bool useSsl )
        {
            if ( _tcpClient == null )
            {
                _tcpClient = new TcpClient( );
                await _tcpClient.ConnectAsync( hostName, port ).ConfigureAwait( false );

                if ( useSsl )
                {
                    _stream = new SslStream( _tcpClient.GetStream( ), false );
#if FULL
                    ( (SslStream)_stream ).AuthenticateAsClient( hostName );
#else
                    await ( (SslStream)_stream ).AuthenticateAsClientAsync( hostName ).ConfigureAwait( false );
#endif
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
                throw new InvalidOperationException( "The Network Stream is null" );

            byte[] data = new byte[ 1 ];
            StringBuilder sb = new StringBuilder( );
            UTF8Encoding enc = new UTF8Encoding( );

            while ( true )
            {
                int dataLength = await _stream.ReadAsync( data, 0, 1 );
                if ( dataLength != 1 )
                    break;

                sb.Append( enc.GetString( data, 0, 1 ) );

                if ( data[ 0 ] == '\n' )
                    break;
            }

            return sb.ToString( );
        }

        public async Task WriteAsync( string data )
        {
            if ( _stream == null )
                throw new InvalidOperationException( "The Network Stream is null" );

            UTF8Encoding en = new UTF8Encoding( );
            byte[] writeBuffer = en.GetBytes( data );

            await _stream.WriteAsync( writeBuffer, 0, writeBuffer.Length ).ConfigureAwait( false );
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