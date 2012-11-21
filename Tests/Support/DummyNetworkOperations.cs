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

namespace Pop3.Tests.Support
{
    public class DummyNetworkOperations : BaseDummyNetworkOperations
    {
        #region Private Vars

        private string _result;

        #endregion

        #region Constructors

        public DummyNetworkOperations( string result )
        {
            _result = result;
        }

        #endregion

        #region BaseDummyNetworkOperations Methods

        protected override string GetData( )
        {
            return _result;
        }

        #endregion
    }
}