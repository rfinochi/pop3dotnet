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
using System.Collections.ObjectModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using Pop3;
using Pop3.IO;

namespace Pop3.Tests
{
    [TestClass]
    public class Pop3ClientFixture
    {
        #region Private Vars

        private Mock<INetworkOperations> _mockNetworkOperations = new Mock<INetworkOperations>( );
        private Mock<INetworkOperations> _mockNetworkOperationsOnlyHeaders = new Mock<INetworkOperations>( );

        #endregion

        #region Setup

        [TestInitialize( )]
        public void Init( )
        {
            _mockNetworkOperations.SetupSequence( no => no.Read( ) )
                                                .Returns( "+OK" )
                                                .Returns( "+OK" )
                                                .Returns( "+OK" )
                                                .Returns( "+OK" )
                                                .Returns( "1 1586\r\n" )
                                                .Returns( "2 1584\r\n" )
                                                .Returns( ".\r\n" )
                                                .Returns( "+OK" )
                                                .Returns( "Delivered-To: rfinochi@shockbyte.net\r\n" )
                                                .Returns( "Received: by 10.112.163.194 with SMTP id yk2csp334265lbb;\r\n" )
                                                .Returns( "Tue, 13 Nov 2012 07:57:16 -0800 (PST)\r\n" )
                                                .Returns( "Received: by 10.68.189.70 with SMTP id gg6mr39046644pbc.97.1352822235497;\r\n" )
                                                .Returns( "        Tue, 13 Nov 2012 07:57:15 -0800 (PST)\r\n" )
                                                .Returns( "Return-Path: <rodolfof@lagash.com>\r\n" )
                                                .Returns( "Received: from relay.ihostexchange.net (relay.ihostexchange.net. [66.46.182.55])\r\n" )
                                                .Returns( "        by mx.google.com with ESMTPS id oi10si14138527pbb.278.2012.11.13.07.57.13\r\n" )
                                                .Returns( "        (version=TLSv1/SSLv3 cipher=OTHER);\r\n" )
                                                .Returns( "        Tue, 13 Nov 2012 07:57:15 -0800 (PST)\r\n" )
                                                .Returns( "Received-SPF: neutral (google.com: 66.46.182.55 is neither permitted nor denied by domain of rodolfof@lagash.com) client-ip=66.46.182.55;\r\n" )
                                                .Returns( "Authentication-Results: mx.google.com; spf=neutral (google.com: 66.46.182.55 is neither permitted nor denied by domain of rodolfof@lagash.com) smtp.mail=rodolfof@lagash.com\r\n" )
                                                .Returns( "Received: from VMBX107.ihostexchange.net ([192.168.3.7]) by\r\n" )
                                                .Returns( " hub105.ihostexchange.net ([66.46.182.55]) with mapi; Tue, 13 Nov 2012\r\n" )
                                                .Returns( " 10:57:12 -0500\r\n" )
                                                .Returns( "From: Rodolfo Finochietti <rodolfof@lagash.com>\r\n" )
                                                .Returns( "To: \"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>\r\n" )
                                                .Returns( "Date: Tue, 13 Nov 2012 10:57:04 -0500\r\n" )
                                                .Returns( "Subject: Test 1\r\n" )
                                                .Returns( "Thread-Topic: Test 1\r\n" )
                                                .Returns( "Thread-Index: Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==\r\n" )
                                                .Returns( "Message-ID: <CCC7F420.6251%rodolfof@lagash.com>\r\n" )
                                                .Returns( "Accept-Language: en-US\r\n" )
                                                .Returns( "Content-Language: en-US\r\n" )
                                                .Returns( "X-MS-Has-Attach:\r\n" )
                                                .Returns( "X-MS-TNEF-Correlator:\r\n" )
                                                .Returns( "user-agent: Microsoft-MacOutlook/14.2.4.120824\r\n" )
                                                .Returns( "acceptlanguage: en-US\r\n" )
                                                .Returns( "Content-Type: text/plain; charset=\"us-ascii\"\r\n" )
                                                .Returns( "Content-Transfer-Encoding: quoted-printable\r\n" )
                                                .Returns( "MIME-Version: 1.0\r\n" )
                                                .Returns( "\r\n" )
                                                .Returns( "Test One\r\n" )
                                                .Returns( "\r\n" )
                                                .Returns( ".\r\n" )
                                                .Returns( "+OK" )
                                                .Returns( "Delivered-To: rfinochi2@shockbyte.net\r\n" )
                                                .Returns( "Received: by 10.112.163.194 with SMTP id yk2csp334323lbb;\r\n" )
                                                .Returns( "        Tue, 13 Nov 2012 07:57:41 -0800 (PST)\r\n" )
                                                .Returns( "Received: by 10.68.213.33 with SMTP id np1mr68271235pbc.64.1352822261234;\r\n" )
                                                .Returns( "        Tue, 13 Nov 2012 07:57:41 -0800 (PST)\r\n" )
                                                .Returns( "Return-Path: <rodolfof2@lagash.com>\r\n" )
                                                .Returns( "Received: from relay.ihostexchange.net (relay.ihostexchange.net. [66.46.182.53])\r\n" )
                                                .Returns( "        by mx.google.com with ESMTPS id h9si14146629pax.184.2012.11.13.07.57.39\r\n" )
                                                .Returns( "        (version=TLSv1/SSLv3 cipher=OTHER);\r\n" )
                                                .Returns( "        Tue, 13 Nov 2012 07:57:40 -0800 (PST)\r\n" )
                                                .Returns( "Received-SPF: neutral (google.com: 66.46.182.53 is neither permitted nor denied by domain of rodolfof@lagash.com) client-ip=66.46.182.53;\r\n" )
                                                .Returns( "Authentication-Results: mx.google.com; spf=neutral (google.com: 66.46.182.53 is neither permitted nor denied by domain of rodolfof@lagash.com) smtp.mail=rodolfof@lagash.com\r\n" )
                                                .Returns( "Received: from VMBX107.ihostexchange.net ([192.168.3.7]) by\r\n" )
                                                .Returns( " HUB103.ihostexchange.net ([66.46.182.53]) with mapi; Tue, 13 Nov 2012\r\n" )
                                                .Returns( " 10:57:35 -0500\r\n" )
                                                .Returns( "From: Rodolfo Finochietti <rodolfof2@lagash.com>\r\n" )
                                                .Returns( "To: \"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>\r\n" )
                                                .Returns( "Date: Tue, 13 Nov 2012 10:57:28 -0500\r\n" )
                                                .Returns( "Subject: Test 2\r\n" )
                                                .Returns( "Thread-Topic: Test 2\r\n" )
                                                .Returns( "Thread-Index: Ac3Bt5g3UdNgEF+cQmmTFVrfHQgqBw==\r\n" )
                                                .Returns( "Message-ID: <CCC7F438.6253%rodolfof2@lagash.com>\r\n" )
                                                .Returns( "Accept-Language: en-US\r\n" )
                                                .Returns( "Content-Language: en-US\r\n" )
                                                .Returns( "X-MS-Has-Attach:\r\n" )
                                                .Returns( "X-MS-TNEF-Correlator:\r\n" )
                                                .Returns( "user-agent: Microsoft-MacOutlook/14.2.4.120824\r\n" )
                                                .Returns( "acceptlanguage: en-US\r\n" )
                                                .Returns( "Content-Type: text/plain; charset=\"us-ascii\"\r\n" )
                                                .Returns( "Content-Transfer-Encoding: base64\r\n" )
                                                .Returns( "MIME-Version: 1.0\r\n" )
                                                .Returns( "\r\n" )
                                                .Returns( Base64EncodingHelper.Encode( "Test Two\r\n" ) )
                                                .Returns( "\r\n" )
                                                .Returns( ".\r\n" );

            _mockNetworkOperationsOnlyHeaders.SetupSequence( no => no.Read( ) )
                                                   .Returns( "+OK" )
                                                   .Returns( "+OK" )
                                                   .Returns( "+OK" )
                                                   .Returns( "+OK" )
                                                   .Returns( "1 1586\r\n" )
                                                   .Returns( "2 1584\r\n" )
                                                   .Returns( ".\r\n" )
                                                   .Returns( "+OK" )
                                                   .Returns( "Delivered-To: rfinochi@shockbyte.net\r\n" )
                                                   .Returns( "Received: by 10.112.163.194 with SMTP id yk2csp334265lbb;\r\n" )
                                                   .Returns( "Tue, 13 Nov 2012 07:57:16 -0800 (PST)\r\n" )
                                                   .Returns( "Received: by 10.68.189.70 with SMTP id gg6mr39046644pbc.97.1352822235497;\r\n" )
                                                   .Returns( "        Tue, 13 Nov 2012 07:57:15 -0800 (PST)\r\n" )
                                                   .Returns( "Return-Path: <rodolfof@lagash.com>\r\n" )
                                                   .Returns( "Received: from relay.ihostexchange.net (relay.ihostexchange.net. [66.46.182.55])\r\n" )
                                                   .Returns( "        by mx.google.com with ESMTPS id oi10si14138527pbb.278.2012.11.13.07.57.13\r\n" )
                                                   .Returns( "        (version=TLSv1/SSLv3 cipher=OTHER);\r\n" )
                                                   .Returns( "        Tue, 13 Nov 2012 07:57:15 -0800 (PST)\r\n" )
                                                   .Returns( "Received-SPF: neutral (google.com: 66.46.182.55 is neither permitted nor denied by domain of rodolfof@lagash.com) client-ip=66.46.182.55;\r\n" )
                                                   .Returns( "Authentication-Results: mx.google.com; spf=neutral (google.com: 66.46.182.55 is neither permitted nor denied by domain of rodolfof@lagash.com) smtp.mail=rodolfof@lagash.com\r\n" )
                                                   .Returns( "Received: from VMBX107.ihostexchange.net ([192.168.3.7]) by\r\n" )
                                                   .Returns( " hub105.ihostexchange.net ([66.46.182.55]) with mapi; Tue, 13 Nov 2012\r\n" )
                                                   .Returns( " 10:57:12 -0500\r\n" )
                                                   .Returns( "From: Rodolfo Finochietti <rodolfof@lagash.com>\r\n" )
                                                   .Returns( "To: \"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>\r\n" )
                                                   .Returns( "Date: Tue, 13 Nov 2012 10:57:04 -0500\r\n" )
                                                   .Returns( "Subject: Test 1\r\n" )
                                                   .Returns( "Thread-Topic: Test 1\r\n" )
                                                   .Returns( "Thread-Index: Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==\r\n" )
                                                   .Returns( "Message-ID: <CCC7F420.6251%rodolfof@lagash.com>\r\n" )
                                                   .Returns( "Accept-Language: en-US\r\n" )
                                                   .Returns( "Content-Language: en-US\r\n" )
                                                   .Returns( "X-MS-Has-Attach:\r\n" )
                                                   .Returns( "X-MS-TNEF-Correlator:\r\n" )
                                                   .Returns( "user-agent: Microsoft-MacOutlook/14.2.4.120824\r\n" )
                                                   .Returns( "acceptlanguage: en-US\r\n" )
                                                   .Returns( "Content-Type: text/plain; charset=\"us-ascii\"\r\n" )
                                                   .Returns( "Content-Transfer-Encoding: quoted-printable\r\n" )
                                                   .Returns( "MIME-Version: 1.0\r\n" )
                                                   .Returns( ".\r\n" )
                                                   .Returns( "+OK" )
                                                   .Returns( "Delivered-To: rfinochi2@shockbyte.net\r\n" )
                                                   .Returns( "Received: by 10.112.163.194 with SMTP id yk2csp334323lbb;\r\n" )
                                                   .Returns( "        Tue, 13 Nov 2012 07:57:41 -0800 (PST)\r\n" )
                                                   .Returns( "Received: by 10.68.213.33 with SMTP id np1mr68271235pbc.64.1352822261234;\r\n" )
                                                   .Returns( "        Tue, 13 Nov 2012 07:57:41 -0800 (PST)\r\n" )
                                                   .Returns( "Return-Path: <rodolfof2@lagash.com>\r\n" )
                                                   .Returns( "Received: from relay.ihostexchange.net (relay.ihostexchange.net. [66.46.182.53])\r\n" )
                                                   .Returns( "        by mx.google.com with ESMTPS id h9si14146629pax.184.2012.11.13.07.57.39\r\n" )
                                                   .Returns( "        (version=TLSv1/SSLv3 cipher=OTHER);\r\n" )
                                                   .Returns( "        Tue, 13 Nov 2012 07:57:40 -0800 (PST)\r\n" )
                                                   .Returns( "Received-SPF: neutral (google.com: 66.46.182.53 is neither permitted nor denied by domain of rodolfof@lagash.com) client-ip=66.46.182.53;\r\n" )
                                                   .Returns( "Authentication-Results: mx.google.com; spf=neutral (google.com: 66.46.182.53 is neither permitted nor denied by domain of rodolfof@lagash.com) smtp.mail=rodolfof@lagash.com\r\n" )
                                                   .Returns( "Received: from VMBX107.ihostexchange.net ([192.168.3.7]) by\r\n" )
                                                   .Returns( " HUB103.ihostexchange.net ([66.46.182.53]) with mapi; Tue, 13 Nov 2012\r\n" )
                                                   .Returns( " 10:57:35 -0500\r\n" )
                                                   .Returns( "From: Rodolfo Finochietti <rodolfof2@lagash.com>\r\n" )
                                                   .Returns( "To: \"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>\r\n" )
                                                   .Returns( "Date: Tue, 13 Nov 2012 10:57:28 -0500\r\n" )
                                                   .Returns( "Subject: Test 2\r\n" )
                                                   .Returns( "Thread-Topic: Test 2\r\n" )
                                                   .Returns( "Thread-Index: Ac3Bt5g3UdNgEF+cQmmTFVrfHQgqBw==\r\n" )
                                                   .Returns( "Message-ID: <CCC7F438.6253%rodolfof2@lagash.com>\r\n" )
                                                   .Returns( "Accept-Language: en-US\r\n" )
                                                   .Returns( "Content-Language: en-US\r\n" )
                                                   .Returns( "X-MS-Has-Attach:\r\n" )
                                                   .Returns( "X-MS-TNEF-Correlator:\r\n" )
                                                   .Returns( "user-agent: Microsoft-MacOutlook/14.2.4.120824\r\n" )
                                                   .Returns( "acceptlanguage: en-US\r\n" )
                                                   .Returns( "Content-Type: text/plain; charset=\"us-ascii\"\r\n" )
                                                   .Returns( "Content-Transfer-Encoding: quoted-printable\r\n" )
                                                   .Returns( "MIME-Version: 1.0\r\n" )
                                                   .Returns( ".\r\n" );
        }

        #endregion

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
            Pop3Client pop3Client = new Pop3Client( _mockNetworkOperations.Object );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );
            Assert.IsTrue( pop3Client.IsConnected );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( Pop3Exception ) )]
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
        [ExpectedException( typeof( Pop3Exception ) )]
        public void ConnectFailNotResponse( )
        {
            Mock<INetworkOperations> mockNetworkOperations = new Mock<INetworkOperations>( );

            Pop3Client pop3Client = new Pop3Client( mockNetworkOperations.Object );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );
        }
        
        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( Pop3Exception ) )]
        public void ConnectAlreadyConnect( )
        {
            Pop3Client pop3Client = new Pop3Client( _mockNetworkOperations.Object );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD", true );
            Assert.IsTrue( pop3Client.IsConnected );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD", 995, true );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void DisconnectOk( )
        {
            Pop3Client pop3Client = new Pop3Client( _mockNetworkOperations.Object );
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
            Pop3Client pop3Client = new Pop3Client( _mockNetworkOperations.Object );
            Assert.IsFalse( pop3Client.IsConnected );

            pop3Client.Disconnect( );
            Assert.IsFalse( pop3Client.IsConnected );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void ListOk( )
        {
            Pop3Client pop3Client = new Pop3Client( _mockNetworkOperations.Object );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD", 995, true );

            Collection<Pop3Message> messages = pop3Client.List( );
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
        [ExpectedException( typeof( Pop3Exception ) )]
        public void ListFail( )
        {
            Pop3Client pop3Client = new Pop3Client( _mockNetworkOperations.Object );

            pop3Client.List( );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void RetrieveOk( )
        {
            Pop3Client pop3Client = new Pop3Client( _mockNetworkOperations.Object );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );

            Collection<Pop3Message> messages = pop3Client.List( );
 
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
            Assert.IsNotNull( messages[ 1 ].GetBodyData( ) );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        [ExpectedException( typeof( Pop3Exception ) )]
        public void RetrieveFail( )
        {
            Pop3Client pop3Client = new Pop3Client( _mockNetworkOperations.Object );

            pop3Client.Retrieve( new Pop3Message( ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void RetrieveListOk( )
        {
            Pop3Client pop3Client = new Pop3Client( _mockNetworkOperations.Object );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );

            Collection<Pop3Message> messages = pop3Client.List( );

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
            Assert.IsNotNull( messages[ 1 ].GetBodyData( ) );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void ListAndRetrieveOk( )
        {
            Pop3Client pop3Client = new Pop3Client( _mockNetworkOperations.Object );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );

            Collection<Pop3Message> messages = pop3Client.ListAndRetrieve( );
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
            Assert.IsNotNull( messages[ 1 ].GetBodyData( ) );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void RetrieveHeaderOk( )
        {
            Pop3Client pop3Client = new Pop3Client( _mockNetworkOperationsOnlyHeaders.Object );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );

            Collection<Pop3Message> messages = pop3Client.List( );

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
            Assert.AreEqual( String.Empty, messages[ 0 ].Body );
            Assert.IsNull( messages[ 0 ].GetBodyData( ) );
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
            Assert.AreEqual( "quoted-printable", messages[ 1 ].ContentTransferEncoding );
            Assert.AreEqual( String.Empty, messages[ 1 ].Body );
            Assert.IsNull( messages[ 1 ].GetBodyData( ) );
            Assert.AreEqual( "Microsoft-MacOutlook/14.2.4.120824", messages[ 1 ].GetHeaderData( "user-agent:" ) );
            Assert.AreEqual( String.Empty, messages[ 1 ].GetHeaderData( "X-MS-Has-Attach:" ) );
        }

        [TestMethod]
        [Owner( "Rodolfo Finochietti" )]
        public void RetrieveHeaderListOk( )
        {
            Pop3Client pop3Client = new Pop3Client( _mockNetworkOperations.Object );

            pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD" );

            Collection<Pop3Message> messages = pop3Client.List( );

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
            Assert.AreEqual( String.Empty, messages[ 0 ].Body );
            Assert.IsNull( messages[ 0 ].GetBodyData( ) );
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
            Assert.AreEqual( String.Empty, messages[ 1 ].Body );
            Assert.IsNull( messages[ 1 ].GetBodyData( ) );
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
        [ExpectedException( typeof( Pop3Exception ) )]
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
        [ExpectedException( typeof( Pop3Exception ) )]
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
    }
}