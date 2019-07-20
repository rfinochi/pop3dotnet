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
using System.Text;

namespace Pop3
{
    internal static class Base64EncodingHelper
    {
        #region Public Methods

        public static string Encode( string data )
        {
            return Encode( Encoding.UTF8.GetBytes( data ) );
        }

        public static string Encode( byte[] data )
        {
            return Convert.ToBase64String( data );
        }
        
        public static string Decode( string data )
        {
            if ( String.IsNullOrEmpty( data ) )
                return String.Empty;
            else
                return Decode( Convert.FromBase64String( data ) );
        }

        public static string Decode( byte[] data )
        {
            if ( data == null || data == new byte[ 0 ] )
            {
                return String.Empty;
            }
            else
            {
                UTF8Encoding encoder = new UTF8Encoding( );
                Decoder utf8Decode = encoder.GetDecoder( );

                int charCount = utf8Decode.GetCharCount( data, 0, data.Length );

                char[] decodedChar = new char[ charCount ];
                utf8Decode.GetChars( data, 0, data.Length, decodedChar, 0 );

                return new String( decodedChar );
            }
        }

        #endregion
    }
}