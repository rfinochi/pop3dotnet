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
using System.Globalization;

namespace Pop3.Tests.Support
{
    public class OnlyHeadersDummyNetworkOperations : QueueDummyNetworkOperations
    {
        #region QueueDummyNetworkOperations Methods

        protected override Queue<string> InitData()
        {
            Queue<string> results = new Queue<string>();

            results.Enqueue("+OK");
            results.Enqueue("+OK");
            results.Enqueue("+OK");
            results.Enqueue("+OK");
            results.Enqueue("1 1586\r\n");
            results.Enqueue("2 1584\r\n");
            results.Enqueue(".\r\n");
            results.Enqueue("+OK");

            results.Enqueue("Delivered-To: rfinochi@shockbyte.net\r\n");
            results.Enqueue("Received: by 10.112.163.194 with SMTP id yk2csp334265lbb;\r\n");
            results.Enqueue("Tue, 13 Nov 2012 07:57:16 -0800 (PST)\r\n");
            results.Enqueue("Received: by 10.68.189.70 with SMTP id gg6mr39046644pbc.97.1352822235497;\r\n");
            results.Enqueue("        Tue, 13 Nov 2012 07:57:15 -0800 (PST)\r\n");
            results.Enqueue("Return-Path: <rodolfof@lagash.com>\r\n");
            results.Enqueue("Received: from relay.ihostexchange.net (relay.ihostexchange.net. [66.46.182.55])\r\n");
            results.Enqueue("        by mx.google.com with ESMTPS id oi10si14138527pbb.278.2012.11.13.07.57.13\r\n");
            results.Enqueue("        (version=TLSv1/SSLv3 cipher=OTHER);\r\n");
            results.Enqueue("        Tue, 13 Nov 2012 07:57:15 -0800 (PST)\r\n");
            results.Enqueue("Received-SPF: neutral (google.com: 66.46.182.55 is neither permitted nor denied by domain of rodolfof@lagash.com) client-ip=66.46.182.55;\r\n");
            results.Enqueue("Authentication-Results: mx.google.com; spf=neutral (google.com: 66.46.182.55 is neither permitted nor denied by domain of rodolfof@lagash.com) smtp.mail=rodolfof@lagash.com\r\n");
            results.Enqueue("Received: from VMBX107.ihostexchange.net ([192.168.3.7]) by\r\n");
            results.Enqueue(" hub105.ihostexchange.net ([66.46.182.55]) with mapi; Tue, 13 Nov 2012\r\n");
            results.Enqueue(" 10:57:12 -0500\r\n");
            results.Enqueue("From: Rodolfo Finochietti <rodolfof@lagash.com>\r\n");
            results.Enqueue("To: \"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>\r\n");
            results.Enqueue("Date: Tue, 13 Nov 2012 10:57:04 -0500\r\n");
            results.Enqueue("Subject: Test 1\r\n");
            results.Enqueue("Thread-Topic: Test 1\r\n");
            results.Enqueue("Thread-Index: Ac3Bt4nMDtM3y3FyQ1yd71JVtsSGJQ==\r\n");
            results.Enqueue("Message-ID: <CCC7F420.6251%rodolfof@lagash.com>\r\n");
            results.Enqueue("Accept-Language: en-US\r\n");
            results.Enqueue("Content-Language: en-US\r\n");
            results.Enqueue("X-MS-Has-Attach:\r\n");
            results.Enqueue("X-MS-TNEF-Correlator:\r\n");
            results.Enqueue("user-agent: Microsoft-MacOutlook/14.2.4.120824\r\n");
            results.Enqueue("acceptlanguage: en-US\r\n");
            results.Enqueue("Content-Type: text/plain; charset=\"us-ascii\"\r\n");
            results.Enqueue(String.Format(CultureInfo.InvariantCulture, "Content-Type-Custom: multipart/mixed;{0}{1}boundary=\"--boundary_0_........-....-....-....-............\"\r\n", "\r\n", " "));
            results.Enqueue("Content-Transfer-Encoding: quoted-printable\r\n");
            results.Enqueue("MIME-Version: 1.0\r\n");
            results.Enqueue(".\r\n");

            results.Enqueue("+OK");

            results.Enqueue("Delivered-To: rfinochi2@shockbyte.net\r\n");
            results.Enqueue("Received: by 10.112.163.194 with SMTP id yk2csp334323lbb;\r\n");
            results.Enqueue("        Tue, 13 Nov 2012 07:57:41 -0800 (PST)\r\n");
            results.Enqueue("Received: by 10.68.213.33 with SMTP id np1mr68271235pbc.64.1352822261234;\r\n");
            results.Enqueue("        Tue, 13 Nov 2012 07:57:41 -0800 (PST)\r\n");
            results.Enqueue("Return-Path: <rodolfof2@lagash.com>\r\n");
            results.Enqueue("Received: from relay.ihostexchange.net (relay.ihostexchange.net. [66.46.182.53])\r\n");
            results.Enqueue("        by mx.google.com with ESMTPS id h9si14146629pax.184.2012.11.13.07.57.39\r\n");
            results.Enqueue("        (version=TLSv1/SSLv3 cipher=OTHER);\r\n");
            results.Enqueue("        Tue, 13 Nov 2012 07:57:40 -0800 (PST)\r\n");
            results.Enqueue("Received-SPF: neutral (google.com: 66.46.182.53 is neither permitted nor denied by domain of rodolfof@lagash.com) client-ip=66.46.182.53;\r\n");
            results.Enqueue("Authentication-Results: mx.google.com; spf=neutral (google.com: 66.46.182.53 is neither permitted nor denied by domain of rodolfof@lagash.com) smtp.mail=rodolfof@lagash.com\r\n");
            results.Enqueue("Received: from VMBX107.ihostexchange.net ([192.168.3.7]) by\r\n");
            results.Enqueue(" HUB103.ihostexchange.net ([66.46.182.53]) with mapi; Tue, 13 Nov 2012\r\n");
            results.Enqueue(" 10:57:35 -0500\r\n");
            results.Enqueue("From: Rodolfo Finochietti <rodolfof2@lagash.com>\r\n");
            results.Enqueue("To: \"rfinochi2@shockbyte.net\" <rfinochi2@shockbyte.net>\r\n");
            results.Enqueue("Date: Tue, 13 Nov 2012 10:57:28 -0500\r\n");
            results.Enqueue("Subject: Test 2\r\n");
            results.Enqueue("Thread-Topic: Test 2\r\n");
            results.Enqueue("Thread-Index: Ac3Bt5g3UdNgEF+cQmmTFVrfHQgqBw==\r\n");
            results.Enqueue("Message-ID: <CCC7F438.6253%rodolfof2@lagash.com>\r\n");
            results.Enqueue("Accept-Language: en-US\r\n");
            results.Enqueue("Content-Language: en-US\r\n");
            results.Enqueue("X-MS-Has-Attach:\r\n");
            results.Enqueue("X-MS-TNEF-Correlator:\r\n");
            results.Enqueue("user-agent: Microsoft-MacOutlook/14.2.4.120824\r\n");
            results.Enqueue("acceptlanguage: en-US\r\n");
            results.Enqueue("Content-Type: text/plain; charset=\"us-ascii\"\r\n");
            results.Enqueue("Content-Transfer-Encoding: base64\r\n");
            results.Enqueue("MIME-Version: 1.0\r\n");
            results.Enqueue(".\r\n");

            return results;
        }

        #endregion
    }
}