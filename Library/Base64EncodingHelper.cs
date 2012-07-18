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

        public static string Decode(string data)
        {
            var todecodeByte = Convert.FromBase64String(data);

            return Decode(todecodeByte);
        }

        public static string Decode(byte[] todecodeByte)
        {
            var encoder = new UTF8Encoding();
            var utf8Decode = encoder.GetDecoder();

            var charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);

            var decodedChar = new char[charCount];
            utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);

            return new String(decodedChar);
        }

        public static string Encode(string data)
        {
            var encDataByte = Encoding.UTF8.GetBytes(data);
            return Decode(encDataByte);
        }

        public static string Encode(byte[] encDataByte)
        {
            return Convert.ToBase64String(encDataByte);
        }

        #endregion
    }
}