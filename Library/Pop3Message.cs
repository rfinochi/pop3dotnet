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

namespace Pop3
{
#if NETFX_CORE
    public sealed class Pop3Message
#else
    public class Pop3Message
#endif
    {
        #region Constants

        private const string FromHeader = "from:";
        private const string ToHeader = "to:";
        private const string DateHeader = "date:";
        private const string MessageIdHeader = "message-id:";
        private const string SubjectHeader = "subject:";
        private const string ContentTransferEncodingHeader = "content-transfer-encoding:";

        #endregion

        #region Private Fields

        private byte[] _bodyData;

        #endregion

        #region Properties

        public long Number
        {
            get;
            set;
        }
        
        public long Bytes
        {
            get;
            set;
        }
        
        public bool Retrieved
        {
            get;
            set;
        }
        
        public string RawHeader
        {
            get;
            set;
        }
        
        public string RawMessage
        {
            get;
            set;
        }

        private string _from;
        
        public string From
        {
            get
            {
                if ( String.IsNullOrEmpty( _from ) )
                    _from = GetHeaderData( FromHeader );

                return _from;
            }
        }

        private string _to;
 
        public string To
        {
            get
            {
                if ( String.IsNullOrEmpty( _to ) )
                    _to = GetHeaderData( ToHeader );

                return _to;
            }
        }

        private string _date;
        
        public string Date
        {
            get
            {
                if ( String.IsNullOrEmpty( _date ) )
                    _date = GetHeaderData( DateHeader );

                return _date;
            }
        }

        private string _messageId;

        public string MessageId
        {
            get
            {
                if ( String.IsNullOrEmpty( _messageId ) )
                    _messageId = GetHeaderData( MessageIdHeader );

                return _messageId;
            }
        }

        private string _subject;

        public string Subject
        {
            get
            {
                if ( String.IsNullOrEmpty( _subject ) )
                    _subject = GetHeaderData( SubjectHeader );

                return _subject;
            }
        }

        private string _contentTransferEncoding;

        public string ContentTransferEncoding
        {
            get
            {
                if ( String.IsNullOrEmpty( _contentTransferEncoding ) )
                    _contentTransferEncoding = GetHeaderData( ContentTransferEncodingHeader );

                return _contentTransferEncoding;
            }
        }

        private string _body;

        public string Body
        {
            get
            {
                if ( String.IsNullOrEmpty( _body ) )
                    _body = GetBody( );

                return _body;
            }
        }

        #endregion

        #region Public Methods

        public string GetHeaderData( string headerName )
        {
            if ( String.IsNullOrEmpty( headerName ) )
                throw new ArgumentNullException( "headerName" );
            if ( String.IsNullOrEmpty( RawMessage ) && String.IsNullOrEmpty( RawHeader ) )
                throw new InvalidOperationException( "Header can't be null" );

            if ( !headerName.EndsWith( ":", StringComparison.OrdinalIgnoreCase  ) )
                headerName += ":";
            
            string result = String.IsNullOrEmpty( RawHeader ) ? RawMessage : RawHeader;

            if ( result == null )
                return null;

            int index = result.IndexOf( String.Format( CultureInfo.InvariantCulture, "\r\n{0}", headerName ), StringComparison.OrdinalIgnoreCase );

            if ( index < 0 )
                return null;

            result = result.Remove( 0, ( index + headerName.Length + 2 ) );

            return result.Remove( result.IndexOf( '\r' ), ( result.Length - result.IndexOf( '\r' ) ) ).Replace( "\n", String.Empty ).Trim( );
        }

        public byte[] GetBodyData( )
        {
            if ( _bodyData == null && !String.IsNullOrEmpty( _body ) )
            {
                UTF8Encoding enc = new UTF8Encoding( );
                _bodyData = enc.GetBytes( _body );
            }

            return _bodyData;
        }

        #endregion

        #region Private Methods

        private string GetBody( )
        {
            if ( String.IsNullOrEmpty( RawMessage ) )
            {
                return String.Empty;
            }
            else
            {
                string body = RawMessage.Remove( 0, ( RawMessage.IndexOf( "\r\n\r\n", StringComparison.OrdinalIgnoreCase ) ) );

                return String.Compare( ContentTransferEncoding, "base64", StringComparison.OrdinalIgnoreCase ) == 0 ? Base64EncodingHelper.Decode( body ) : body;
            }
        }

        #endregion
    }
}