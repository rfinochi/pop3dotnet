/*
 * Author: Rodolfo Finochietti
 * Email: rfinochi@shockbyte.net
 * Web: http://shockbyte.net
 *
 * This work is licensed under the Creative Commons Attribution License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by/2.0
 * or send a letter to Creative Commons, 559 Nathan Abbott Way, Stanford, California 94305, USA.
 * 
 * Portions of this sample are copyright Microsoft Corporation.
 * 
 * Portions of this sample are copyright Steve Maine (Email: stevem@hyperthink.net, Web: http://hyperthink.net/blog).
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

        public static string Decode( string data )
        {
            return Decode( Convert.FromBase64String( data ) );
        }

        public static string Decode( byte[] todecodeByte )
        {
            UTF8Encoding encoder = new UTF8Encoding( );
            Decoder utf8Decode = encoder.GetDecoder( );

            int charCount = utf8Decode.GetCharCount( todecodeByte, 0, todecodeByte.Length );

            char[] decodedChar = new char[ charCount ];
            utf8Decode.GetChars( todecodeByte, 0, todecodeByte.Length, decodedChar, 0 );

            return new String( decodedChar );
        }

        public static string Encode( string data )
        {
            return Decode( Encoding.UTF8.GetBytes( data ) );
        }

        public static string Encode( byte[] encDataByte )
        {
            return Convert.ToBase64String( encDataByte );
        }

        #endregion
    }
}