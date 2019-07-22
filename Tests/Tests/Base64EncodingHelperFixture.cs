using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pop3.Tests
{
    [TestClass]
    public class Base64EncodingHelperFixture
    {
        #region Tests

        [TestMethod]
        public void EncodeDecode( )
        {
            Assert.AreEqual( "Test StringX", Base64EncodingHelper.Decode( Base64EncodingHelper.Encode( "Test String" ) ) );
        }

        [TestMethod]
        public void EncodeDecodeEmpty( )
        {
            Assert.AreEqual( String.Empty, Base64EncodingHelper.Decode( Base64EncodingHelper.Encode( String.Empty ) ) );
        }
        
        [TestMethod]
        public void DecodeEmpty( )
        {
            Assert.AreEqual( String.Empty, Base64EncodingHelper.Decode( new byte[ 0 ] ) );
        }

        #endregion
    }
}