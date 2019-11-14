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
using System.Collections.Generic;

namespace Pop3.Tests.Support
{
    public class DummyNetworkOperations : BaseDummyNetworkOperations
    {
        #region Private Vars

        private string _result;

        #endregion

        #region Constructors

        public DummyNetworkOperations(string result)
        {
            _result = result;
        }

        #endregion

        #region BaseDummyNetworkOperations Methods

        protected override string GetData()
        {
            return _result;
        }

        #endregion
    }
}