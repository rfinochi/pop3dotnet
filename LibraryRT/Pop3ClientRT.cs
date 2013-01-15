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
using System.Collections.Generic;

using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace Pop3
{
    public sealed class Pop3Client : IDisposable
    {
        #region Private Fields

        private InternalPop3Client _client = new InternalPop3Client( );

        #endregion

        #region Properties

        public bool IsConnected
        {
            get
            {
                return _client.IsConnected;
            }
        }

        #endregion

        #region Public Async Methods

        public IAsyncAction ConnectAsync( string server, string userName, string password )
        {
            return _client.ConnectAsync( server, userName, password, 110, false ).AsAsyncAction( );
        }

        public IAsyncAction ConnectAsync( string server, string userName, string password, bool useSsl )
        {
            return _client.ConnectAsync( server, userName, password, ( useSsl ? 995 : 110 ), useSsl ).AsAsyncAction( );
        }

        public IAsyncAction ConnectAsync( string server, string userName, string password, int port, bool useSsl )
        {
            return _client.ConnectAsync( server, userName, password, port, useSsl ).AsAsyncAction( );
        }

        public IAsyncAction DisconnectAsync( )
        {
            return _client.DisconnectAsync( ).AsAsyncAction( );
        }

        public IAsyncOperation<IEnumerable<Pop3Message>> ListAsync( )
        {
            return _client.ListAsync( ).AsAsyncOperation( );
        }

        [DefaultOverload]
        public IAsyncAction RetrieveHeaderAsync( Pop3Message message )
        {
            return _client.RetrieveHeaderAsync( message ).AsAsyncAction( );
        }

        public IAsyncAction RetrieveHeaderAsync( IEnumerable<Pop3Message> messages )
        {
            return _client.RetrieveHeaderAsync( new List<Pop3Message>( messages ) ).AsAsyncAction( );
        }

        [DefaultOverload]
        public IAsyncAction RetrieveAsync( Pop3Message message )
        {
            return _client.RetrieveAsync( message ).AsAsyncAction( );
        }

        public IAsyncAction RetrieveAsync( IEnumerable<Pop3Message> messages )
        {
            return _client.RetrieveAsync( new List<Pop3Message>( messages ) ).AsAsyncAction( );
        }

        public IAsyncOperation<IEnumerable<Pop3Message>> ListAndRetrieveHeaderAsync( )
        {
            return _client.ListAndRetrieveHeaderAsync( ).AsAsyncOperation( );
        }

        public IAsyncOperation<IEnumerable<Pop3Message>> ListAndRetrieveAsync( )
        {
            return _client.ListAndRetrieveAsync( ).AsAsyncOperation( );
        }

        public IAsyncAction DeleteAsync( Pop3Message message )
        {
            return _client.DeleteAsync( message ).AsAsyncAction( );
        }

        #endregion

        #region Dispose-Finalize Pattern

        public void Dispose( )
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        ~Pop3Client( )
        {
            Dispose( false );
        }

        private void Dispose( bool disposing )
        {
            if ( disposing )
                _client.Dispose( );
        }

        #endregion
    }
}