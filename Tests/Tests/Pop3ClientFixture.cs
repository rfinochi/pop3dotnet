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
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Pop3.IO;
using Pop3.Tests.Support;

namespace Pop3.Tests
{
    [TestClass]
    public class Pop3ClientFixture
    {
        #region Tests

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void CreateFailNullNetworkOperation( )
        {
            Pop3Client pop3Client = new Pop3Client( null );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void ConnectOk( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );
            Assert.IsTrue( pop3Client.IsConnected );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public void ConnectFail( )
        {
            Mock<INetworkOperations> mockNetworkOperations = new Mock<INetworkOperations>( );
            mockNetworkOperations.Setup( no => no.Read( ) ).Returns( "-ERR" );

            Pop3Client pop3Client = new Pop3Client( mockNetworkOperations.Object );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD", true );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public void ConnectFailNotResponse( )
        {
            Mock<INetworkOperations> mockNetworkOperations = new Mock<INetworkOperations>( );

            Pop3Client pop3Client = new Pop3Client( mockNetworkOperations.Object );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public void ConnectAlreadyConnect( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD", true );
            Assert.IsTrue( pop3Client.IsConnected );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD", 995, true );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void DisconnectOk( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );
            Assert.IsTrue( pop3Client.IsConnected );

            pop3Client.Disconnect( );
            Assert.IsFalse( pop3Client.IsConnected );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void DisconnectFailNotConnect( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Disconnect( );
            Assert.IsFalse( pop3Client.IsConnected );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void ListOk( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD", 995, true );

            List<Pop3Message> messages = new List<Pop3Message>( pop3Client.List( ) );
            Assert.AreEqual( 2, messages.Count );
            Assert.AreEqual( 1, messages[ 0 ].Number );
            Assert.AreEqual( 1586, messages[ 0 ].Bytes );
            Assert.IsFalse( messages[ 0 ].Retrieved );
            Assert.AreEqual( 2, messages[ 1 ].Number );
            Assert.AreEqual( 1584, messages[ 1 ].Bytes );
            Assert.IsFalse( messages[ 1 ].Retrieved );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public void ListFail( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );

            pop3Client.List( );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void RetrieveOk( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );

            List<Pop3Message> messages = new List<Pop3Message>( pop3Client.List( ) );

            pop3Client.Retrieve( messages[ 0 ] );
            Assert.IsTrue( messages[ 0 ].Retrieved );
            Assert.IsNull( messages[ 0 ].RawHeader );
            Assert.IsNotNull( messages[ 0 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof@lagash.com>", messages[ 0 ].From );
            Assert.AreEqual( "\"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>", messages[ 0 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:04 -0500", messages[ 0 ].Date );
            Assert.AreEqual( "<CCC7F420.6251%rodolfof@lagash.com>", messages[ 0 ].MessageId );
            Assert.AreEqual( "Test 1", messages[ 0 ].Subject );
            Assert.AreEqual( "quoted-printable", messages[ 0 ].ContentTransferEncoding );
            Assert.AreEqual( "\r\n\r\nTest One\r\n\r\n", messages[ 0 ].Body );
            Assert.AreEqual( "1.0", messages[ 0 ].GetHeaderData( "MIME-Version" ) );
            Assert.AreEqual( "Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==", messages[ 0 ].GetHeaderData( "Thread-Index" ) );

            pop3Client.Retrieve( messages[ 1 ] );
            Assert.IsTrue( messages[ 1 ].Retrieved );
            Assert.IsNull( messages[ 1 ].RawHeader );
            Assert.IsNotNull( messages[ 1 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof2@lagash.com>", messages[ 1 ].From );
            Assert.AreEqual( "\"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>", messages[ 1 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:28 -0500", messages[ 1 ].Date );
            Assert.AreEqual( "<CCC7F438.6253%rodolfof2@lagash.com>", messages[ 1 ].MessageId );
            Assert.AreEqual( "Test 2", messages[ 1 ].Subject );
            Assert.AreEqual( "base64", messages[ 1 ].ContentTransferEncoding );
            Assert.AreEqual( "Test Two\r\n", messages[ 1 ].Body );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public void RetrieveFail( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );

            pop3Client.Retrieve( new Pop3Message( ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void RetrieveListOk( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );

            List<Pop3Message> messages = new List<Pop3Message>( pop3Client.List( ) );

            pop3Client.Retrieve( messages );
            Assert.IsTrue( messages[ 0 ].Retrieved );
            Assert.IsNull( messages[ 0 ].RawHeader );
            Assert.IsNotNull( messages[ 0 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof@lagash.com>", messages[ 0 ].From );
            Assert.AreEqual( "\"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>", messages[ 0 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:04 -0500", messages[ 0 ].Date );
            Assert.AreEqual( "<CCC7F420.6251%rodolfof@lagash.com>", messages[ 0 ].MessageId );
            Assert.AreEqual( "Test 1", messages[ 0 ].Subject );
            Assert.AreEqual( "quoted-printable", messages[ 0 ].ContentTransferEncoding );
            Assert.AreEqual( "\r\n\r\nTest One\r\n\r\n", messages[ 0 ].Body );
            Assert.AreEqual( "1.0", messages[ 0 ].GetHeaderData( "MIME-Version" ) );
            Assert.AreEqual( "Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==", messages[ 0 ].GetHeaderData( "Thread-Index" ) );
            Assert.IsTrue( messages[ 1 ].Retrieved );
            Assert.IsNull( messages[ 1 ].RawHeader );
            Assert.IsNotNull( messages[ 1 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof2@lagash.com>", messages[ 1 ].From );
            Assert.AreEqual( "\"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>", messages[ 1 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:28 -0500", messages[ 1 ].Date );
            Assert.AreEqual( "<CCC7F438.6253%rodolfof2@lagash.com>", messages[ 1 ].MessageId );
            Assert.AreEqual( "Test 2", messages[ 1 ].Subject );
            Assert.AreEqual( "base64", messages[ 1 ].ContentTransferEncoding );
            Assert.AreEqual( "Test Two\r\n", messages[ 1 ].Body );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void ListAndRetrieveOk( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );

            List<Pop3Message> messages = new List<Pop3Message>( pop3Client.ListAndRetrieve( ) );
            Assert.IsTrue( messages[ 0 ].Retrieved );
            Assert.IsNull( messages[ 0 ].RawHeader );
            Assert.IsNotNull( messages[ 0 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof@lagash.com>", messages[ 0 ].From );
            Assert.AreEqual( "\"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>", messages[ 0 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:04 -0500", messages[ 0 ].Date );
            Assert.AreEqual( "<CCC7F420.6251%rodolfof@lagash.com>", messages[ 0 ].MessageId );
            Assert.AreEqual( "Test 1", messages[ 0 ].Subject );
            Assert.AreEqual( "quoted-printable", messages[ 0 ].ContentTransferEncoding );
            Assert.AreEqual( "\r\n\r\nTest One\r\n\r\n", messages[ 0 ].Body );
            Assert.AreEqual( "1.0", messages[ 0 ].GetHeaderData( "MIME-Version" ) );
            Assert.AreEqual( "Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==", messages[ 0 ].GetHeaderData( "Thread-Index" ) );
            Assert.IsTrue( messages[ 1 ].Retrieved );
            Assert.IsNull( messages[ 1 ].RawHeader );
            Assert.IsNotNull( messages[ 1 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof2@lagash.com>", messages[ 1 ].From );
            Assert.AreEqual( "\"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>", messages[ 1 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:28 -0500", messages[ 1 ].Date );
            Assert.AreEqual( "<CCC7F438.6253%rodolfof2@lagash.com>", messages[ 1 ].MessageId );
            Assert.AreEqual( "Test 2", messages[ 1 ].Subject );
            Assert.AreEqual( "base64", messages[ 1 ].ContentTransferEncoding );
            Assert.AreEqual( "Test Two\r\n", messages[ 1 ].Body );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void ListAndRetrieveHeaderOk( )
        {
            Pop3Client pop3Client = new Pop3Client( new OnlyHeadersDummyNetworkOperations( ) );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );

            List<Pop3Message> messages = new List<Pop3Message>( pop3Client.ListAndRetrieveHeader( ) );
            Assert.IsFalse( messages[ 0 ].Retrieved );
            Assert.IsNotNull( messages[ 0 ].RawHeader );
            Assert.IsNull( messages[ 0 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof@lagash.com>", messages[ 0 ].From );
            Assert.AreEqual( "\"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>", messages[ 0 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:04 -0500", messages[ 0 ].Date );
            Assert.AreEqual( "<CCC7F420.6251%rodolfof@lagash.com>", messages[ 0 ].MessageId );
            Assert.AreEqual( "quoted-printable", messages[ 0 ].ContentTransferEncoding );
            Assert.IsNull( messages[ 0 ].Body );
            Assert.AreEqual( "1.0", messages[ 0 ].GetHeaderData( "MIME-Version" ) );
            Assert.AreEqual( "Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==", messages[ 0 ].GetHeaderData( "Thread-Index" ) );
            Assert.IsFalse( messages[ 1 ].Retrieved );
            Assert.IsNotNull( messages[ 1 ].RawHeader );
            Assert.IsNull( messages[ 1 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof2@lagash.com>", messages[ 1 ].From );
            Assert.AreEqual( "\"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>", messages[ 1 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:28 -0500", messages[ 1 ].Date );
            Assert.AreEqual( "<CCC7F438.6253%rodolfof2@lagash.com>", messages[ 1 ].MessageId );
            Assert.AreEqual( "Test 2", messages[ 1 ].Subject );
            Assert.AreEqual( "base64", messages[ 1 ].ContentTransferEncoding );
            Assert.IsNull( messages[ 1 ].Body );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void RetrieveHeaderOk( )
        {
            Pop3Client pop3Client = new Pop3Client( new OnlyHeadersDummyNetworkOperations( ) );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );

            List<Pop3Message> messages = new List<Pop3Message>( pop3Client.List( ) );

            pop3Client.RetrieveHeader( messages[ 0 ] );
            Assert.IsFalse( messages[ 0 ].Retrieved );
            Assert.IsNotNull( messages[ 0 ].RawHeader );
            Assert.IsNull( messages[ 0 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof@lagash.com>", messages[ 0 ].From );
            Assert.AreEqual( "\"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>", messages[ 0 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:04 -0500", messages[ 0 ].Date );
            Assert.AreEqual( "<CCC7F420.6251%rodolfof@lagash.com>", messages[ 0 ].MessageId );
            Assert.AreEqual( "Test 1", messages[ 0 ].Subject );
            Assert.AreEqual( "quoted-printable", messages[ 0 ].ContentTransferEncoding );
            Assert.IsNull( messages[ 0 ].Body );
            Assert.AreEqual( "1.0", messages[ 0 ].GetHeaderData( "MIME-Version" ) );
            Assert.AreEqual( "Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==", messages[ 0 ].GetHeaderData( "Thread-Index" ) );

            pop3Client.RetrieveHeader( messages[ 1 ] );
            Assert.IsFalse( messages[ 1 ].Retrieved );
            Assert.IsNotNull( messages[ 1 ].RawHeader );
            Assert.IsNull( messages[ 1 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof2@lagash.com>", messages[ 1 ].From );
            Assert.AreEqual( "\"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>", messages[ 1 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:28 -0500", messages[ 1 ].Date );
            Assert.AreEqual( "<CCC7F438.6253%rodolfof2@lagash.com>", messages[ 1 ].MessageId );
            Assert.AreEqual( "Test 2", messages[ 1 ].Subject );
            Assert.AreEqual( "base64", messages[ 1 ].ContentTransferEncoding );
            Assert.IsNull( messages[ 1 ].Body );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void RetrieveFoldedHeaderOk( )
        {
            Pop3Client pop3Client = new Pop3Client( new OnlyHeadersDummyNetworkOperations( ) );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );

            List<Pop3Message> messages = new List<Pop3Message>( pop3Client.List( ) );

            pop3Client.RetrieveHeader( messages[ 0 ] );
            Assert.IsFalse( messages[ 0 ].Retrieved );
            Assert.IsNotNull( messages[ 0 ].RawHeader );
            Assert.IsNull( messages[ 0 ].RawMessage );
            Assert.AreEqual( "multipart/mixed;boundary=\"--boundary_0_........-....-....-....-............\"", messages[ 0 ].GetHeaderData( "Content-Type-Custom" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void RetrieveHeaderListOk( )
        {
            Pop3Client pop3Client = new Pop3Client( new OnlyHeadersDummyNetworkOperations( ) );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );

            List<Pop3Message> messages = new List<Pop3Message>( pop3Client.List( ) );

            pop3Client.RetrieveHeader( messages );
            Assert.IsFalse( messages[ 0 ].Retrieved );
            Assert.IsNotNull( messages[ 0 ].RawHeader );
            Assert.IsNull( messages[ 0 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof@lagash.com>", messages[ 0 ].From );
            Assert.AreEqual( "\"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>", messages[ 0 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:04 -0500", messages[ 0 ].Date );
            Assert.AreEqual( "<CCC7F420.6251%rodolfof@lagash.com>", messages[ 0 ].MessageId );
            Assert.AreEqual( "Test 1", messages[ 0 ].Subject );
            Assert.AreEqual( "quoted-printable", messages[ 0 ].ContentTransferEncoding );
            Assert.IsNull( messages[ 0 ].Body );
            Assert.AreEqual( "1.0", messages[ 0 ].GetHeaderData( "MIME-Version" ) );
            Assert.AreEqual( "Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==", messages[ 0 ].GetHeaderData( "Thread-Index" ) );
            Assert.IsFalse( messages[ 1 ].Retrieved );
            Assert.IsNotNull( messages[ 1 ].RawHeader );
            Assert.IsNull( messages[ 1 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof2@lagash.com>", messages[ 1 ].From );
            Assert.AreEqual( "\"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>", messages[ 1 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:28 -0500", messages[ 1 ].Date );
            Assert.AreEqual( "<CCC7F438.6253%rodolfof2@lagash.com>", messages[ 1 ].MessageId );
            Assert.AreEqual( "Test 2", messages[ 1 ].Subject );
            Assert.AreEqual( "base64", messages[ 1 ].ContentTransferEncoding );
            Assert.IsNull( messages[ 1 ].Body );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void DeleteOk( )
        {
            Mock<INetworkOperations> mockNetworkOperations = new Mock<INetworkOperations>( );
            mockNetworkOperations.Setup( no => no.Read( ) ).Returns( "+OK" );

            Pop3Client pop3Client = new Pop3Client( mockNetworkOperations.Object );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD", false );

            pop3Client.Delete( new Pop3Message( ) { Number = 1 } );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public void DeleteFailNotConnect( )
        {
            Mock<INetworkOperations> mockNetworkOperations = new Mock<INetworkOperations>( );
            mockNetworkOperations.Setup( no => no.Read( ) ).Returns( "+OK" );

            Pop3Client pop3Client = new Pop3Client( mockNetworkOperations.Object );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Delete( new Pop3Message( ) { Number = 1 } );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public void DeleteFailNullArgument( )
        {
            Mock<INetworkOperations> mockNetworkOperations = new Mock<INetworkOperations>( );
            mockNetworkOperations.Setup( no => no.Read( ) ).Returns( "+OK" );

            Pop3Client pop3Client = new Pop3Client( mockNetworkOperations.Object );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD", true );
            Assert.IsTrue( pop3Client.IsConnected );

            pop3Client.Delete( null );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public void SendCommandFailNullResponse( )
        {
            Mock<INetworkOperations> mockNetworkOperations = new Mock<INetworkOperations>( );
            mockNetworkOperations.SetupSequence( no => no.Read( ) )
                                                .Returns( "+OK" )
                                                .Returns( String.Empty );

            Pop3Client pop3Client = new Pop3Client( mockNetworkOperations.Object );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD", false );
        }

        #endregion

        #region Async Tests

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public async Task ConnectOkAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );
            Assert.IsFalse( pop3Client.IsConnected );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD" );
            Assert.IsTrue( pop3Client.IsConnected );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public async Task ConnectFailAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new DummyNetworkOperations( "-ERR" ) );
            Assert.IsFalse( pop3Client.IsConnected );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD", true );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public async Task ConnectFailNotResponseAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new DummyNetworkOperations( null ) );
            Assert.IsFalse( pop3Client.IsConnected );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD" );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public async Task ConnectAlreadyConnectAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );
            Assert.IsFalse( pop3Client.IsConnected );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD", true );
            Assert.IsTrue( pop3Client.IsConnected );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD", 995, true );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public async Task DisconnectOkAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );
            Assert.IsFalse( pop3Client.IsConnected );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD" );
            Assert.IsTrue( pop3Client.IsConnected );

            await pop3Client.DisconnectAsync( );
            Assert.IsFalse( pop3Client.IsConnected );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public async Task DisconnectFailNotConnectAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );
            Assert.IsFalse( pop3Client.IsConnected );

            await pop3Client.DisconnectAsync( );
            Assert.IsFalse( pop3Client.IsConnected );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public async Task ListOkAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD", 995, true );

            IEnumerable<Pop3Message> result = await pop3Client.ListAsync( );
            List<Pop3Message> messages = new List<Pop3Message>( result );

            Assert.AreEqual( 2, messages.Count );
            Assert.AreEqual( 1, messages[ 0 ].Number );
            Assert.AreEqual( 1586, messages[ 0 ].Bytes );
            Assert.IsFalse( messages[ 0 ].Retrieved );
            Assert.AreEqual( 2, messages[ 1 ].Number );
            Assert.AreEqual( 1584, messages[ 1 ].Bytes );
            Assert.IsFalse( messages[ 1 ].Retrieved );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public async Task ListFailAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );

            await pop3Client.ListAsync( );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public async Task RetrieveOkAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD" );

            IEnumerable<Pop3Message> result = await pop3Client.ListAsync( );
            List<Pop3Message> messages = new List<Pop3Message>( result ); ;

            await pop3Client.RetrieveAsync( messages[ 0 ] );
            Assert.IsTrue( messages[ 0 ].Retrieved );
            Assert.IsNull( messages[ 0 ].RawHeader );
            Assert.IsNotNull( messages[ 0 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof@lagash.com>", messages[ 0 ].From );
            Assert.AreEqual( "\"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>", messages[ 0 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:04 -0500", messages[ 0 ].Date );
            Assert.AreEqual( "<CCC7F420.6251%rodolfof@lagash.com>", messages[ 0 ].MessageId );
            Assert.AreEqual( "Test 1", messages[ 0 ].Subject );
            Assert.AreEqual( "quoted-printable", messages[ 0 ].ContentTransferEncoding );
            Assert.AreEqual( "\r\n\r\nTest One\r\n\r\n", messages[ 0 ].Body );
            Assert.AreEqual( "1.0", messages[ 0 ].GetHeaderData( "MIME-Version" ) );
            Assert.AreEqual( "Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==", messages[ 0 ].GetHeaderData( "Thread-Index" ) );

            await pop3Client.RetrieveAsync( messages[ 1 ] );
            Assert.IsTrue( messages[ 1 ].Retrieved );
            Assert.IsNull( messages[ 1 ].RawHeader );
            Assert.IsNotNull( messages[ 1 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof2@lagash.com>", messages[ 1 ].From );
            Assert.AreEqual( "\"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>", messages[ 1 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:28 -0500", messages[ 1 ].Date );
            Assert.AreEqual( "<CCC7F438.6253%rodolfof2@lagash.com>", messages[ 1 ].MessageId );
            Assert.AreEqual( "Test 2", messages[ 1 ].Subject );
            Assert.AreEqual( "base64", messages[ 1 ].ContentTransferEncoding );
            Assert.AreEqual( "Test Two\r\n", messages[ 1 ].Body );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public async Task RetrieveFailAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );

            await pop3Client.RetrieveAsync( new Pop3Message( ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public async Task RetrieveListOkAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD" );

            IEnumerable<Pop3Message> result = await pop3Client.ListAsync( );
            List<Pop3Message> messages = new List<Pop3Message>( result );

            await pop3Client.RetrieveAsync( messages );
            Assert.IsTrue( messages[ 0 ].Retrieved );
            Assert.IsNull( messages[ 0 ].RawHeader );
            Assert.IsNotNull( messages[ 0 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof@lagash.com>", messages[ 0 ].From );
            Assert.AreEqual( "\"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>", messages[ 0 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:04 -0500", messages[ 0 ].Date );
            Assert.AreEqual( "<CCC7F420.6251%rodolfof@lagash.com>", messages[ 0 ].MessageId );
            Assert.AreEqual( "Test 1", messages[ 0 ].Subject );
            Assert.AreEqual( "quoted-printable", messages[ 0 ].ContentTransferEncoding );
            Assert.AreEqual( "\r\n\r\nTest One\r\n\r\n", messages[ 0 ].Body );
            Assert.AreEqual( "1.0", messages[ 0 ].GetHeaderData( "MIME-Version" ) );
            Assert.AreEqual( "Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==", messages[ 0 ].GetHeaderData( "Thread-Index" ) );
            Assert.IsTrue( messages[ 1 ].Retrieved );
            Assert.IsNull( messages[ 1 ].RawHeader );
            Assert.IsNotNull( messages[ 1 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof2@lagash.com>", messages[ 1 ].From );
            Assert.AreEqual( "\"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>", messages[ 1 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:28 -0500", messages[ 1 ].Date );
            Assert.AreEqual( "<CCC7F438.6253%rodolfof2@lagash.com>", messages[ 1 ].MessageId );
            Assert.AreEqual( "Test 2", messages[ 1 ].Subject );
            Assert.AreEqual( "base64", messages[ 1 ].ContentTransferEncoding );
            Assert.AreEqual( "Test Two\r\n", messages[ 1 ].Body );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public async Task ListAndRetrieveOkAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new PlainMessagesDummyNetworkOperations( ) );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD" );

            IEnumerable<Pop3Message> result = await pop3Client.ListAndRetrieveAsync( );
            List<Pop3Message> messages = new List<Pop3Message>( result );

            Assert.IsTrue( messages[ 0 ].Retrieved );
            Assert.IsNull( messages[ 0 ].RawHeader );
            Assert.IsNotNull( messages[ 0 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof@lagash.com>", messages[ 0 ].From );
            Assert.AreEqual( "\"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>", messages[ 0 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:04 -0500", messages[ 0 ].Date );
            Assert.AreEqual( "<CCC7F420.6251%rodolfof@lagash.com>", messages[ 0 ].MessageId );
            Assert.AreEqual( "Test 1", messages[ 0 ].Subject );
            Assert.AreEqual( "quoted-printable", messages[ 0 ].ContentTransferEncoding );
            Assert.AreEqual( "\r\n\r\nTest One\r\n\r\n", messages[ 0 ].Body );
            Assert.AreEqual( "1.0", messages[ 0 ].GetHeaderData( "MIME-Version" ) );
            Assert.AreEqual( "Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==", messages[ 0 ].GetHeaderData( "Thread-Index" ) );
            Assert.IsTrue( messages[ 1 ].Retrieved );
            Assert.IsNull( messages[ 1 ].RawHeader );
            Assert.IsNotNull( messages[ 1 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof2@lagash.com>", messages[ 1 ].From );
            Assert.AreEqual( "\"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>", messages[ 1 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:28 -0500", messages[ 1 ].Date );
            Assert.AreEqual( "<CCC7F438.6253%rodolfof2@lagash.com>", messages[ 1 ].MessageId );
            Assert.AreEqual( "Test 2", messages[ 1 ].Subject );
            Assert.AreEqual( "base64", messages[ 1 ].ContentTransferEncoding );
            Assert.AreEqual( "Test Two\r\n", messages[ 1 ].Body );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public async Task ListAndRetrieveHeaderOkAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new OnlyHeadersDummyNetworkOperations( ) );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD" );

            IEnumerable<Pop3Message> result = await pop3Client.ListAndRetrieveHeaderAsync( );
            List<Pop3Message> messages = new List<Pop3Message>( result ); ;

            Assert.IsFalse( messages[ 0 ].Retrieved );
            Assert.IsNotNull( messages[ 0 ].RawHeader );
            Assert.IsNull( messages[ 0 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof@lagash.com>", messages[ 0 ].From );
            Assert.AreEqual( "\"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>", messages[ 0 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:04 -0500", messages[ 0 ].Date );
            Assert.AreEqual( "<CCC7F420.6251%rodolfof@lagash.com>", messages[ 0 ].MessageId );
            Assert.AreEqual( "quoted-printable", messages[ 0 ].ContentTransferEncoding );
            Assert.IsNull( messages[ 0 ].Body );
            Assert.AreEqual( "1.0", messages[ 0 ].GetHeaderData( "MIME-Version" ) );
            Assert.AreEqual( "Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==", messages[ 0 ].GetHeaderData( "Thread-Index" ) );
            Assert.IsFalse( messages[ 1 ].Retrieved );
            Assert.IsNotNull( messages[ 1 ].RawHeader );
            Assert.IsNull( messages[ 1 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof2@lagash.com>", messages[ 1 ].From );
            Assert.AreEqual( "\"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>", messages[ 1 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:28 -0500", messages[ 1 ].Date );
            Assert.AreEqual( "<CCC7F438.6253%rodolfof2@lagash.com>", messages[ 1 ].MessageId );
            Assert.AreEqual( "Test 2", messages[ 1 ].Subject );
            Assert.AreEqual( "base64", messages[ 1 ].ContentTransferEncoding );
            Assert.IsNull( messages[ 1 ].Body );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public async Task RetrieveHeaderListOkAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new OnlyHeadersDummyNetworkOperations( ) );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD" );

            IEnumerable<Pop3Message> result = await pop3Client.ListAsync( );
            List<Pop3Message> messages = new List<Pop3Message>( result );

            await pop3Client.RetrieveHeaderAsync( messages );
            Assert.IsFalse( messages[ 0 ].Retrieved );
            Assert.IsNotNull( messages[ 0 ].RawHeader );
            Assert.IsNull( messages[ 0 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof@lagash.com>", messages[ 0 ].From );
            Assert.AreEqual( "\"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>", messages[ 0 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:04 -0500", messages[ 0 ].Date );
            Assert.AreEqual( "<CCC7F420.6251%rodolfof@lagash.com>", messages[ 0 ].MessageId );
            Assert.AreEqual( "Test 1", messages[ 0 ].Subject );
            Assert.AreEqual( "quoted-printable", messages[ 0 ].ContentTransferEncoding );
            Assert.IsNull( messages[ 0 ].Body );
            Assert.AreEqual( "1.0", messages[ 0 ].GetHeaderData( "MIME-Version" ) );
            Assert.AreEqual( "Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==", messages[ 0 ].GetHeaderData( "Thread-Index" ) );
            Assert.IsFalse( messages[ 1 ].Retrieved );
            Assert.IsNotNull( messages[ 1 ].RawHeader );
            Assert.IsNull( messages[ 1 ].RawMessage );
            Assert.AreEqual( "Rodolfo Finochietti <rodolfof2@lagash.com>", messages[ 1 ].From );
            Assert.AreEqual( "\"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>", messages[ 1 ].To );
            Assert.AreEqual( "Tue, 13 Nov 2012 10:57:28 -0500", messages[ 1 ].Date );
            Assert.AreEqual( "<CCC7F438.6253%rodolfof2@lagash.com>", messages[ 1 ].MessageId );
            Assert.AreEqual( "Test 2", messages[ 1 ].Subject );
            Assert.AreEqual( "base64", messages[ 1 ].ContentTransferEncoding );
            Assert.IsNull( messages[ 1 ].Body );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public async Task DeleteOkAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new DummyNetworkOperations( "+OK" ) );
            Assert.IsFalse( pop3Client.IsConnected );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD", false );

            await pop3Client.DeleteAsync( new Pop3Message( ) { Number = 1 } );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public async Task DeleteFailNotConnectAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new DummyNetworkOperations( "+OK" ) );
            Assert.IsFalse( pop3Client.IsConnected );

            await pop3Client.DeleteAsync( new Pop3Message( ) { Number = 1 } );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( ArgumentNullException ) )]
        public async Task DeleteFailNullArgumentAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new DummyNetworkOperations( "+OK" ) );
            Assert.IsFalse( pop3Client.IsConnected );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD", true );
            Assert.IsTrue( pop3Client.IsConnected );

            await pop3Client.DeleteAsync( null );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( InvalidOperationException ) )]
        public async Task SendCommandFailNullResponseAsync( )
        {
            Pop3Client pop3Client = new Pop3Client( new QueueDummyNetworkOperations( "+OK", String.Empty ) );
            Assert.IsFalse( pop3Client.IsConnected );

            await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD", false );
        }

        #endregion
    }
}