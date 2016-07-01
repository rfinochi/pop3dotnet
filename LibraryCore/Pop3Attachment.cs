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

namespace Pop3
{
    public sealed class Pop3Attachment
    {
        #region Properties

        private byte[] _data;
        
        #endregion

        #region Constructors

        internal Pop3Attachment( string name, string attachmentType, byte[] data )
        {
            this.Name = name;
            this.AttachmentType = attachmentType;
            _data = data;
        }

        #endregion

        #region Properties

        public string Name
        {
            get;
            private set;
        }

        public string AttachmentType
        {
            get;
            private set;
        }

        #endregion

        #region Public Methods

        public byte[] GetData( )
        {
            return _data;
        }

        #endregion
    }
}