/*
 * Author: Rodolfo Finochietti
 * Email: rfinochi@shockbyte.net
 * Web: http://shockbyte.net
 *
 * This work is licensed under the MIT License. 
 * 
 * No warranties expressed or implied, use at your own risk.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Pop3.IO;

namespace Pop3
{
#if RT
    internal class InternalPop3Client : IPop3Client
#else
    public class Pop3Client : IPop3Client
#endif
    {
        #region Private Fields

        private INetworkOperations _networkOperations;

        #endregion

        #region Constructors

#if RT
        public InternalPop3Client( )
        {
            _networkOperations = new StreamSocketNetworkOperations( );
        }
#else
        public Pop3Client( )
        {
            _networkOperations = new TcpClientNetworkOperations( );
        }

        internal Pop3Client( INetworkOperations networkOperations )
        {
            if ( networkOperations == null )
                throw new ArgumentNullException( "networkOperations", "The parameter networkOperation can't be null" );

            _networkOperations = networkOperations;
        }
#endif
        #endregion

        #region Properties

        public bool IsConnected
        {
            get;
            private set;
        }

        #endregion

        #region Public Methods

#if FULL
        public void Connect( string server, string userName, string password )
        {
            Connect( server, userName, password, 110, false, true );
        }

        public void Connect( string server, string userName, string password, bool useSsl )
        {
            Connect( server, userName, password, ( useSsl ? 995 : 110 ), useSsl, true );
        }

        public void Connect( string server, string userName, string password, bool useSsl, bool checkCertificate )
        {
            Connect( server, userName, password, ( useSsl ? 995 : 110 ), useSsl, checkCertificate );
        }
        public void Connect( string server, string userName, string password, int port, bool useSsl )
        {
            Connect( server, userName, password, port, useSsl, true );
        }

        public void Connect( string server, string userName, string password, int port, bool useSsl, bool checkCertificate )
        {
            if ( this.IsConnected )
                throw new InvalidOperationException( "Pop3 client already connected" );

            _networkOperations.Open( server, port, useSsl, checkCertificate );

            string response = _networkOperations.Read( );

            if ( String.IsNullOrEmpty( response ) || response.Substring( 0, 3 ) != "+OK" )
                throw new InvalidOperationException( response );

            SendCommand( String.Format( CultureInfo.InvariantCulture, "USER {0}", userName ) );
            SendCommand( String.Format( CultureInfo.InvariantCulture, "PASS {0}", password ) );

            this.IsConnected = true;
        }

        public void Disconnect( )
        {
            if ( !this.IsConnected )
                return;

            try
            {
                SendCommand( "QUIT" );
                _networkOperations.Close( );
            }
            finally
            {
                this.IsConnected = false;
            }
        }

        public IEnumerable<Pop3Message> List( )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );

            List<Pop3Message> result = new List<Pop3Message>( );

            SendCommand( "LIST" );

            while ( true )
            {
                string response = _networkOperations.Read( );
                if ( response == ".\r\n" )
                    return result.AsEnumerable( );

                Pop3Message message = new Pop3Message( );

                char[] seps = { ' ' };
                string[] values = response.Split( seps );

                message.Number = Int32.Parse( values[ 0 ], CultureInfo.InvariantCulture );
                message.Bytes = Int32.Parse( values[ 1 ], CultureInfo.InvariantCulture );
                message.Retrieved = false;

                result.Add( message );
            }
        }

        public void RetrieveHeader( Pop3Message message )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );

            if ( message == null )
                throw new ArgumentNullException( "message" );

            SendCommand( "TOP", "0", message );

            StringBuilder rawHeader = new StringBuilder();

            while ( true )
            {
                string response = _networkOperations.Read();
                if ( response == ".\r\n" )
                    break;

                rawHeader.Append( response );
            }

            message.RawHeader = rawHeader.ToString();
        }

        public void RetrieveHeader( IEnumerable<Pop3Message> messages )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );
            if ( messages == null )
                throw new ArgumentNullException( "messages" );

            foreach ( Pop3Message message in messages )
                RetrieveHeader( message );
        }

        public void Retrieve( Pop3Message message )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );
            if ( message == null )
                throw new ArgumentNullException( "message" );

            SendCommand( "RETR", message );

            StringBuilder rawMessage = new StringBuilder( );

            while ( true )
            {
                string response = _networkOperations.Read( );
                if ( response == ".\r\n" )
                    break;

                rawMessage.Append( response );
            }

            message.RawMessage = rawMessage.ToString( );
            message.ParseRawMessage( );

            message.Retrieved = true;
        }

        public void Retrieve( IEnumerable<Pop3Message> messages )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );
            if ( messages == null )
                throw new ArgumentNullException( "messages" );

            foreach ( Pop3Message message in messages )
                Retrieve( message );
        }

        public IEnumerable<Pop3Message> ListAndRetrieveHeader( )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );

            IEnumerable<Pop3Message> messages = List( );

            RetrieveHeader( messages );

            return messages;
        }

        public IEnumerable<Pop3Message> ListAndRetrieve( )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );

            IEnumerable<Pop3Message> messages = List( );

            Retrieve( messages );

            return messages;
        }

        public void Delete( Pop3Message message )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );
            if ( message == null )
                throw new ArgumentNullException( "message" );

            SendCommand( "DELE", message );
        }
#endif

        #endregion

        #region Public Async Methods

#if !NET40
        public async Task ConnectAsync( string server, string userName, string password )
        {
            await ConnectAsync( server, userName, password, 110, false ).ConfigureAwait( false );
        }

        public async Task ConnectAsync( string server, string userName, string password, bool useSsl )
        {
            await ConnectAsync( server, userName, password, ( useSsl ? 995 : 110 ), useSsl ).ConfigureAwait( false );
        }

        public async Task ConnectAsync( string server, string userName, string password, int port, bool useSsl )
        {
            if ( this.IsConnected )
                throw new InvalidOperationException( "Pop3 client already connected" );

            await _networkOperations.OpenAsync( server, port, useSsl ).ConfigureAwait( false );

            string response = await _networkOperations.ReadAsync( );
            if ( String.IsNullOrEmpty( response ) || response.Substring( 0, 3 ) != "+OK" )
                throw new InvalidOperationException( response );

            await SendCommandAsync( String.Format( CultureInfo.InvariantCulture, "USER {0}", userName ) ).ConfigureAwait( false );
            await SendCommandAsync( String.Format( CultureInfo.InvariantCulture, "PASS {0}", password ) ).ConfigureAwait( false );

            this.IsConnected = true;
        }

        public async Task DisconnectAsync( )
        {
            if ( !this.IsConnected )
                return;

            try
            {
                await SendCommandAsync( "QUIT" ).ConfigureAwait( false );
                _networkOperations.Close( );
            }
            finally
            {
                this.IsConnected = false;
            }
        }

        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures" )]
        public async Task<IEnumerable<Pop3Message>> ListAsync( )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );

            List<Pop3Message> result = new List<Pop3Message>( );

            await SendCommandAsync( "LIST" ).ConfigureAwait( false );

            while ( true )
            {
                string response = await _networkOperations.ReadAsync( ).ConfigureAwait( false );
                if ( response == ".\r\n" )
                    return result.AsEnumerable( );

                Pop3Message message = new Pop3Message( );

                char[] seps = { ' ' };
                string[] values = response.Split( seps );

                message.Number = Int32.Parse( values[ 0 ], CultureInfo.InvariantCulture );
                message.Bytes = Int32.Parse( values[ 1 ], CultureInfo.InvariantCulture );
                message.Retrieved = false;

                result.Add( message );
            }
        }

        public async Task RetrieveHeaderAsync( Pop3Message message )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );

            if ( message == null )
                throw new ArgumentNullException( "message" );

            await SendCommandAsync( "TOP", "0", message ).ConfigureAwait( false );

            StringBuilder rawHeader = new StringBuilder();

            while ( true )
            {
                string response = await _networkOperations.ReadAsync( ).ConfigureAwait( false );
                if ( response == ".\r\n" )
                    break;

                rawHeader.Append( response );
            }

            message.RawHeader = rawHeader.ToString();
        }

        public async Task RetrieveHeaderAsync( IEnumerable<Pop3Message> messages )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );

            if ( messages == null )
                throw new ArgumentNullException( "messages" );

            foreach ( Pop3Message message in messages )
                await RetrieveHeaderAsync( message ).ConfigureAwait( false );
        }

        public async Task RetrieveAsync( Pop3Message message )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );

            if ( message == null )
                throw new ArgumentNullException( "message" );

            await SendCommandAsync( "RETR", message ).ConfigureAwait( false );

            StringBuilder rawMessage = new StringBuilder();

            while ( true )
            {
                string response = await _networkOperations.ReadAsync( ).ConfigureAwait( false );
                if ( response == ".\r\n" )
                    break;

                rawMessage.Append( response );
            }

            message.RawMessage = rawMessage.ToString( );
            message.ParseRawMessage( );

            message.Retrieved = true;
        }

        public async Task RetrieveAsync( IEnumerable<Pop3Message> messages )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );
            if ( messages == null )
                throw new ArgumentNullException( "messages" );

            foreach ( Pop3Message message in messages )
                await RetrieveAsync( message ).ConfigureAwait( false );
        }

        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures" )]
        public async Task<IEnumerable<Pop3Message>> ListAndRetrieveHeaderAsync( )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );

            IEnumerable<Pop3Message> messages = await ListAsync( ).ConfigureAwait( false );

            await RetrieveHeaderAsync( messages ).ConfigureAwait( false );

            return messages;
        }

        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures" )]
        public async Task<IEnumerable<Pop3Message>> ListAndRetrieveAsync( )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );

            IEnumerable<Pop3Message> messages = await ListAsync( ).ConfigureAwait( false );

            await RetrieveAsync( messages ).ConfigureAwait( false );

            return messages;
        }

        public async Task DeleteAsync( Pop3Message message )
        {
            if ( !this.IsConnected )
                throw new InvalidOperationException( "Pop3 client is not connected to host" );

            if ( message == null )
                throw new ArgumentNullException( "message" );

            await SendCommandAsync( "DELE", message ).ConfigureAwait( false );
        }
#endif

        #endregion

        #region Private Methods

#if FULL
        private void SendCommand( string command, Pop3Message message )
        {
            SendCommand( command, null, message );
        }

        private void SendCommand( string command, string aditionalParameters = null, Pop3Message message = null )
        {
            var request = new StringBuilder( );

            if ( message == null )
                request.AppendFormat( CultureInfo.InvariantCulture, "{0}", command );
            else
                request.AppendFormat( CultureInfo.InvariantCulture, "{0} {1}", command, message.Number );

            if ( !String.IsNullOrEmpty( aditionalParameters ) )
                request.AppendFormat( CultureInfo.InvariantCulture, " {0}", aditionalParameters );

            request.Append( "\r\n" );

            _networkOperations.Write( request.ToString( ) );

            var response = _networkOperations.Read( );

            if ( String.IsNullOrEmpty( response ) || response.Substring( 0, 3 ) != "+OK" )
                throw new InvalidOperationException( response );
        }
#endif

        #endregion

        #region Private Async Methods

#if !NET40
        private async Task SendCommandAsync( string command, Pop3Message message )
        {
            await SendCommandAsync( command, null, message ).ConfigureAwait( false );
        }

        private async Task SendCommandAsync( string command, string aditionalParameters = null, Pop3Message message = null )
        {
            var request = new StringBuilder( );

            if ( message == null )
                request.AppendFormat( CultureInfo.InvariantCulture, "{0}", command );
            else
                request.AppendFormat( CultureInfo.InvariantCulture, "{0} {1}", command, message.Number );

            if ( !String.IsNullOrEmpty( aditionalParameters ) )
                request.AppendFormat( " {0}", aditionalParameters );

            request.Append( Environment.NewLine );

            await _networkOperations.WriteAsync( request.ToString( ) ).ConfigureAwait( false );

            var response = await _networkOperations.ReadAsync( ).ConfigureAwait( false );

            if ( String.IsNullOrEmpty( response ) || response.Substring( 0, 3 ) != "+OK" )
                throw new InvalidOperationException( response );
        }
#endif

        #endregion

        #region Dispose-Finalize Pattern

        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

#if RT
        ~InternalPop3Client( )
#else
        ~Pop3Client( )
#endif
        {
            Dispose( false );
        }

        protected virtual void Dispose( bool disposing )
        {
            if ( disposing )
            {
                if ( _networkOperations != null )
                {
                    _networkOperations.Close( );
                    _networkOperations.Dispose( );
                    _networkOperations = null;
                }
            }

        }

        #endregion
    }
}