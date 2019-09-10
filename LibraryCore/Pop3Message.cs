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
using System.Globalization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Pop3
{
    public sealed class Pop3Message
    {
        #region Constants

        private const string FromHeader = "from";
        private const string ToHeader = "to";
        private const string ReplyHeader = "reply-to";
        private const string InReplyHeader = "in-reply-to";
        private const string ReferencesHeader = "references";
        private const string DateHeader = "date";
        private const string MessageIdHeader = "message-id";
        private const string SubjectHeader = "subject";
        private const string MimeVersionHeader = "mime-version";
        private const string ContentTypeHeader = "content-type";
        private const string ContentTypeHeaderPlain = "text/plain";
        private const string ContentTypeHeaderHtml = "text/html";
        private const string ContentTransferEncodingHeader = "content-transfer-encoding";

        #endregion

        #region Properties

        public long Number
        {
            get;
            internal set;
        }
        
        public long Bytes
        {
            get;
            internal set;
        }
        
        public bool Retrieved
        {
            get;
            internal set;
        }
        
        public string RawHeader
        {
            get;
            internal set;
        }

        public string RawMessage
        {
            get;
            internal set;
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

        private double _mimeVersion = 0;

        public double MimeVersion
        {
            get
            {
                if ( _mimeVersion == 0 )
                    _mimeVersion = ParseDoble( GetHeaderData( MimeVersionHeader ) );

                return _mimeVersion;
            }
        }

        private string _contentType;

        public string ContentType
        {
            get
            {
                if ( String.IsNullOrEmpty( _contentType ) )
                    ParseContentType( GetHeaderData( ContentTypeHeader ));

                return _contentType;
            }
        }

        private string _contentBoundary;

        public string ContentBoundary
        {
            get
            {
                if ( String.IsNullOrEmpty( _contentType ) )
                    ParseContentType( GetHeaderData( ContentTypeHeader ) );

                return _contentBoundary;
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

        public string Body
        {
            get;
            private set;
        }

        private List<Pop3Attachment> _attachments = new List<Pop3Attachment>( );

        public IEnumerable<Pop3Attachment> Attachments
        {
            get
            {
                return _attachments;
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

            if ( !headerName.EndsWith( ":", StringComparison.OrdinalIgnoreCase ) )
                headerName += ":";

            string result = String.IsNullOrEmpty( RawHeader ) ? RawMessage : RawHeader;

            if ( result == null )
                return null;

            int index = result.IndexOf( String.Format( CultureInfo.CurrentCulture, "\r\n{0}", headerName ), StringComparison.OrdinalIgnoreCase );

            if ( index < 0 )
                return null;

            result = result.Remove( 0, ( index + headerName.Length + 2 ) ).Replace( "\r\n ", "" );

            index = Regex.Match( result, @"^[A-Za-z\-]+\:.*$", RegexOptions.Multiline ).Index;

            if ( index > 0 )
            {
                return result.Remove( index, ( result.Length - index ) ).Trim( );
            }
            else
            {
                index = result.IndexOf( '\r' );
                return result.Remove( index, ( result.Length - index ) ).Replace( "\n", String.Empty ).Trim( );
            }
        }

        internal void ParseRawMessage( )
        {
            string body;

            if ( String.IsNullOrEmpty( this.RawMessage ) )
            {
                body = String.Empty;
            }
            else
            {
                body = RawMessage.Remove( 0, ( this.RawMessage.IndexOf( "\r\n\r\n", StringComparison.OrdinalIgnoreCase ) ) );

                try
                {
                    body = String.Compare( this.ContentTransferEncoding, "base64", StringComparison.OrdinalIgnoreCase ) == 0 ? Base64EncodingHelper.Decode( body ) : body;
                }
                catch ( FormatException )
                {
                    body = RawMessage.Remove( 0, ( this.RawMessage.IndexOf( "\r\n\r\n", StringComparison.OrdinalIgnoreCase ) ) );
                }
            }

            this.Body = body;
        }

        #endregion

        #region Private Methods

        private void ParseContentType( string value )
        {
            string type = value;

            if ( type.Contains( ";" ) )
            {
                string[] parts = type.Split( ';' );

                _contentType = parts[ 0 ];

                for ( int x = 1; x < parts.Length; x++ )
                {
                    parts[ x ] = parts[ x ].Trim( );
                    if ( parts[ x ].StartsWith( "boundary=\"", StringComparison.CurrentCulture ) )
                    {
                        _contentBoundary = parts[ x ].Substring( 10, parts[ x ].Length - 11 );
                    }
                    else if ( parts[ x ].StartsWith( "boundary", StringComparison.CurrentCulture ) )
                    {
                        _contentBoundary = parts[ x ].Substring( 9, parts[ x ].Length - 9 );
                    }
                }
            }
            else
            {
                _contentType = value;
            }
        }

        private static double ParseDoble( string value )
        {
            double fnord;
            if ( double.TryParse( value, out fnord ) )
            {
                return fnord;
            }
            else
            {
                Debug.WriteLine( String.Format( CultureInfo.CurrentCulture, "Double parse error. Original value '{0}'.", value ) );

                return 0;
            }
        }

        #endregion
    }
}