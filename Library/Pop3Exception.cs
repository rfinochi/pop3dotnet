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
using System.Runtime.Serialization;

namespace Pop3
{
    [Serializable]
    public class Pop3Exception : Exception
    {
        #region Constructors

        public Pop3Exception( ) { }

        public Pop3Exception( string message ) : base( message ) { }

        public Pop3Exception( string message, Exception innerException ) : base( message, innerException ) { }

        protected Pop3Exception( SerializationInfo info, StreamingContext context ) : base( info, context ) { }

        #endregion
    }
}