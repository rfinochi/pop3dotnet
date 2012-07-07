/*
 * Author: Rodolfo Finochietti
 * Email: rfinochi@shockbyte.net
 * Web: http://shockbyte.net
 *
 * This work is licensed under the Creative Commons Attribution License. 
 * To view a copy of this license, visit  http://creativecommons.org/licenses/by/2.0
 * or send a letter to Creative Commons, 559 Nathan Abbott Way, Stanford, California 94305, USA.
 * 
 * No warranties expressed or implied, use at your own risk.
 */
using System;
using System.Globalization;
using System.Text;

namespace Pop3
{
    public class Pop3Message
    {
        #region Constants

        private const string FromHeader = "from:";
        private const string ToHeader = "to:";
        private const string DateHeader = "date:";
        private const string MessageIdHeader = "message-id:";
        private const string SubjectHeader = "subject:";
        private const string ContentTransferEncodingHeader = "content-transfer-encoding:";
        
        #endregion

        #region Private Vars

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

        public string Header
        {
            get;
            set;
        }

        public string Message
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
                    _from = GetHeader( FromHeader );

                return _from;
            }
        }

        private string _to;

        public string To
        {
            get
            {
                if ( String.IsNullOrEmpty( _to ) )
                    _to = GetHeader( ToHeader );

                return _to;
            }
        }

        private string _date;

        public string Date
        {
            get
            {
                if ( String.IsNullOrEmpty( _date ) )
                    _date = GetHeader( DateHeader );

                return _date;
            }
        }

        private string _messageId;

        public string MessageId
        {
            get
            {
                if ( String.IsNullOrEmpty( _messageId ) )
                    _messageId = GetHeader( MessageIdHeader );

                return _messageId;
            }
        }

        private string _subject;

        public string Subject
        {
            get
            {
                if ( String.IsNullOrEmpty( _subject ) )
                    _subject = GetHeader( SubjectHeader );

                return _subject;
            }
        }

        private string _contentTransferEncoding;

        public string ContentTransferEncoding
        {
            get
            {
                if ( String.IsNullOrEmpty( _contentTransferEncoding ) )
                    _contentTransferEncoding = GetHeader( ContentTransferEncodingHeader );

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

        private byte[] _bodyData;

        public byte[] BodyData
        {
            get
            {
                if ( _bodyData == null )
                    _bodyData = GetBodyData( );

                return _bodyData;
            }
        }

        #endregion

        #region Constructors

        public Pop3Message( ) { }

        #endregion

        #region Private Methods

        private string GetHeader( string headerName )
        {
            if ( String.IsNullOrEmpty( Message ) && String.IsNullOrEmpty( Header ) )
                throw new Pop3Exception( "Header can't be null" );

            string result;
            if ( String.IsNullOrEmpty( Header ) )
                result = Message;
            else
                result = Header;

            int index = result.IndexOf( String.Format( CultureInfo.InvariantCulture, "\r\n{0}", headerName ), StringComparison.OrdinalIgnoreCase );
            if ( index == -1 )
            {
                result = null;
            }
            else
            {
                result = result.Remove( 0, ( index + headerName.Length + 2 ) );
                result = result.Remove( result.IndexOf( '\r' ), ( result.Length - result.IndexOf( '\r' ) ) ).Replace( "\n", String.Empty ).Trim( );
            }

            return result;
        }
        
        private string GetBody( )
        {
            if ( String.IsNullOrEmpty( Message ) )
                throw new Pop3Exception( "Message can't be null" );

            string body = Message.Remove( 0, ( Message.IndexOf( "\r\n\r\n" ) ) );

            if ( String.Compare( ContentTransferEncoding, "base64", true ) == 0 )
                return Base64EncodingHelper.Decode( body );
            else
                return body;
        }

        private byte[] GetBodyData( )
        {
            ASCIIEncoding enc = new ASCIIEncoding( );

            return enc.GetBytes( GetBody( ) );
        }

        #endregion
    }
}