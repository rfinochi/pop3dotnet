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
    public class QueueDummyNetworkOperations : BaseDummyNetworkOperations
    {
        #region Private Vars

        private Queue<string> _results;

        #endregion

        #region Constructors

        public QueueDummyNetworkOperations()
        {
            _results = InitData();
        }

        public QueueDummyNetworkOperations(params string[] results)
        {
            _results = new Queue<string>();

            foreach (string result in results)
                _results.Enqueue(result);
        }

        #endregion

        #region BaseDummyNetworkOperations Methods

        protected override string GetData()
        {
            if (_results != null && _results.Count > 0)
                return _results.Dequeue();
            else
                return String.Empty;
        }

        #endregion

        #region Private Methods

        protected virtual Queue<string> InitData()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}