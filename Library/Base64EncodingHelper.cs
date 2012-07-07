/*
 * Author: Rodolfo Finochietti
 * Email: rfinochi@shockbyte.net
 * Web: http://shockbyte.net
 *
 * This work is licensed under the Creative Commons Attribution License. 
 * To view a copy of this license, visit  http://creativecommons.org/licenses/by/2.0
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
            UTF8Encoding encoder = new UTF8Encoding( );
            Decoder utf8Decode = encoder.GetDecoder( );

            byte[ ] todecode_byte = Convert.FromBase64String( data );

            return Decode( todecode_byte );
        }

        public static string Decode( byte[ ] todecode_byte )
        {
            UTF8Encoding encoder = new UTF8Encoding( );
            Decoder utf8Decode = encoder.GetDecoder( );

            int charCount = utf8Decode.GetCharCount( todecode_byte, 0, todecode_byte.Length );

            char[ ] decoded_char = new char[ charCount ];
            utf8Decode.GetChars( todecode_byte, 0, todecode_byte.Length, decoded_char, 0 );

            return new String( decoded_char );
        }

        public static string Encode( string data )
        {
            byte[ ] encData_byte = Encoding.UTF8.GetBytes( data );
            return Decode( encData_byte );
        }

        public static string Encode( byte[ ] encData_byte )
        {
            return Convert.ToBase64String( encData_byte );
        }

        #endregion
    }
}