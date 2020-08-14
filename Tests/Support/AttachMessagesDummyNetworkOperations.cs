/*
 * Author: Rodolfo Finochietti
 * Email: rfinochi@shockbyte.net
 * Web: http://shockbyte.net
 *
 * This work is licensed under the MIT License. 
 * 
 * No warranties expressed or implied, use at your own risk.
 */
using System.Collections.Generic;

namespace Pop3.Tests.Support
{
    public class AttachMessagesDummyNetworkOperations : QueueDummyNetworkOperations
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

            results.Enqueue("Delivered-To: rfinochi@shockbyte.net");
            results.Enqueue("Received: by 10.13.208.199 with SMTP id s190csp339585ywd;");
            results.Enqueue("        Thu, 9 Jun 2016 04:57:35 -0700 (PDT)");
            results.Enqueue("X-Received: by 10.202.81.145 with SMTP id f139mr3403650oib.174.1465473455481;");
            results.Enqueue("        Thu, 09 Jun 2016 04:57:35 -0700 (PDT)");
            results.Enqueue("Return-Path: <rfinochi@shockbyte.net>");
            results.Enqueue("Received: from na01-by2-obe.outbound.protection.outlook.com (mail-by2on0056.outbound.protection.outlook.com. [207.46.100.56])");
            results.Enqueue("        by mx.google.com with ESMTPS id y64si1065184otb.218.2016.06.09.04.57.35");
            results.Enqueue("        for <rfinochi@shockbyte.net>");
            results.Enqueue("        (version=TLS1_2 cipher=ECDHE-RSA-AES128-SHA bits=128/128);");
            results.Enqueue("        Thu, 09 Jun 2016 04:57:35 -0700 (PDT)");
            results.Enqueue("Received-SPF: pass (google.com: domain of rfinochi@shockbyte.net designates 207.46.100.56 as permitted sender) client-ip=207.46.100.56;");
            results.Enqueue("Authentication-Results: mx.google.com;");
            results.Enqueue("       dkim=pass header.i=@lagashsystems365.onmicrosoft.com;");
            results.Enqueue("       spf=pass (google.com: domain of rfinochi@shockbyte.net designates 207.46.100.56 as permitted sender) smtp.mailfrom=rfinochi@shockbyte.net");
            results.Enqueue("DKIM-Signature: v=1; a=rsa-sha256; c=relaxed/relaxed;");
            results.Enqueue(" d=lagashsystems365.onmicrosoft.com; s=selector1-lagash-com;");
            results.Enqueue(" h=From:Date:Subject:Message-ID:Content-Type:MIME-Version;");
            results.Enqueue(" bh=M6JACb8dUoeVmxucT/JdzLUmanadE7r1VLdq5SN9Ok8=;");
            results.Enqueue(" b=sbcGUihCfAvbGRdz1AdMm1fzHUksgO2FZebbPV1dt3wvdAey+c6PUGu4zNScjCcOPyDcvctGsUqDOPSuBJFMm6TViWyUHzJxIKlTo6o/jisccI4sD+Zf26ODca4zmAvPIYBPsOXaeGzRiupwWztDP28I1gYkfMW16zjfk8qhnKw=");
            results.Enqueue("Received: from BLUPR06MB099.namprd06.prod.outlook.com (10.255.189.20) by");
            results.Enqueue(" BLUPR06MB097.namprd06.prod.outlook.com (10.255.189.15) with Microsoft SMTP");
            results.Enqueue(" Server (TLS) id 15.1.506.9; Thu, 9 Jun 2016 11:57:34 +0000");
            results.Enqueue("Received: from BLUPR06MB099.namprd06.prod.outlook.com ([169.254.3.242]) by");
            results.Enqueue(" BLUPR06MB099.namprd06.prod.outlook.com ([169.254.3.242]) with mapi id");
            results.Enqueue(" 15.01.0506.016; Thu, 9 Jun 2016 11:57:33 +0000");
            results.Enqueue("From: Rodolfo Finochietti <rfinochi@shockbyte.net>");
            results.Enqueue("To: \"rfinochi@shockbyte.net\" <rfinochi@shockbyte.net>");
            results.Enqueue("Subject: Test Plain Attach");
            results.Enqueue("Thread-Topic: Test Plain Attach");
            results.Enqueue("Thread-Index: AdHCRhUJtWJj1GBeQkCbajNomnMA5w==");
            results.Enqueue("Date: Thu, 9 Jun 2016 11:57:33 +0000");
            results.Enqueue("Message-ID: <BLUPR06MB099CF734A1EC6EA5D8185C8B05F0@BLUPR06MB099.namprd06.prod.outlook.com>");
            results.Enqueue("Accept-Language: en-US");
            results.Enqueue("Content-Language: en-US");
            results.Enqueue("X-MS-Has-Attach: yes");
            results.Enqueue("X-MS-TNEF-Correlator:");
            results.Enqueue("authentication-results: spf=none (sender IP is )");
            results.Enqueue(" smtp.mailfrom=rfinochi@shockbyte.net;");
            results.Enqueue("x-originating-ip: [200.68.122.121]");
            results.Enqueue("x-ms-office365-filtering-correlation-id: f9665bc4-7474-4bf9-3c88-08d3905d3e08");
            results.Enqueue("x-microsoft-exchange-diagnostics: 1;BLUPR06MB097;5:1CNU1DpnXR1uzJUOl4hu32C0XlFRkbEcRmQKuafJv69GKsy7eRs/HxwagnFb4KoqieZcjdNXXb6BkbA4abFWbW/o6SVbH9epzpROxxCmAWAl1pvgh8RStL0FHCXaPLFmv28BxpBIuIn80A4GVe9vSQ==;24:5uAx4WlZkxzbtpJ9oTk7TW/ppDz4QCGRipoTQj7EjzBrdQOOrJbta14VCoEet4Va4cceR9bv0+FaiWV7GdEQDcTfdNoQ2T+sIunGjc5jIkU=;7:aS5Jd6oPwD8qDTUtlC/Je5rfiIwvSlPRoo6kyV5O6QlrXtEIpE6jrK+f1YdxD4v7yj8107KYM+AAX3YaU5g7UAp0jCeZYBigZj9S3O37eTUbA02YDEGx25xFAqvMvO9LVYuYgHPVvhQh1Noh31L59hB+C+sheagXLJhcdj2MIo5Q6u5xmNnAMSz5fKgvMy/JEJ0dvJpW1CMZqA//J4FnynjWnbepqCwp416iFjU5PK4=");
            results.Enqueue("x-microsoft-antispam: UriScan:;BCL:0;PCL:0;RULEID:;SRVR:BLUPR06MB097;");
            results.Enqueue("x-microsoft-antispam-prvs: <BLUPR06MB0974964599836B94774B654B05F0@BLUPR06MB097.namprd06.prod.outlook.com>");
            results.Enqueue("x-exchange-antispam-report-test: UriScan:;");
            results.Enqueue("x-exchange-antispam-report-cfa-test: BCL:0;PCL:0;RULEID:(102415321)(6040130)(601004)(2401047)(5005006)(8121501046)(3002001)(10201501046)(6041072)(6043046);SRVR:BLUPR06MB097;BCL:0;PCL:0;RULEID:;SRVR:BLUPR06MB097;");
            results.Enqueue("x-forefront-prvs: 0968D37274");
            results.Enqueue("x-forefront-antispam-report: SFV:NSPM;SFS:(10009020)(6009001)(199003)(189002)(107886002)(81156014)(54356999)(8676002)(110136002)(5008740100001)(2501003)(3480700004)(81166006)(5002640100001)(5004730100002)(76576001)(558084003)(97736004)(8936002)(5003600100002)(3660700001)(99936001)(3280700002)(50986999)(9686002)(10400500002)(122556002)(68736007)(5640700001)(189998001)(586003)(99286002)(3846002)(102836003)(74316001)(101416001)(2906002)(33656002)(92566002)(229853001)(2351001)(66066001)(106356001)(105586002)(6116002)(450100001)(87936001)(2900100001)(86362001)(122373004);DIR:OUT;SFP:1101;SCL:1;SRVR:BLUPR06MB097;H:BLUPR06MB099.namprd06.prod.outlook.com;FPR:;SPF:None;PTR:InfoNoRecords;A:1;MX:1;LANG:en;");
            results.Enqueue("received-spf: None (protection.outlook.com: lagash.com does not designate");
            results.Enqueue(" permitted sender hosts)");
            results.Enqueue("spamdiagnosticoutput: 1:99");
            results.Enqueue("spamdiagnosticmetadata: NSPM");
            results.Enqueue("Content-Type: multipart/mixed;");
            results.Enqueue("	boundary=\"_002_BLUPR06MB099CF734A1EC6EA5D8185C8B05F0BLUPR06MB099namprd_\"");
            results.Enqueue("MIME-Version: 1.0");
            results.Enqueue("X-OriginatorOrg: lagash.com");
            results.Enqueue("X-MS-Exchange-CrossTenant-originalarrivaltime: 09 Jun 2016 11:57:33.5053");
            results.Enqueue(" (UTC)");
            results.Enqueue("X-MS-Exchange-CrossTenant-fromentityheader: Hosted");
            results.Enqueue("X-MS-Exchange-CrossTenant-id: 5ab9af9b-4534-4c31-8e50-1e098461481c");
            results.Enqueue("X-MS-Exchange-Transport-CrossTenantHeadersStamped: BLUPR06MB097");
            results.Enqueue("");
            results.Enqueue("--_002_BLUPR06MB099CF734A1EC6EA5D8185C8B05F0BLUPR06MB099namprd_");
            results.Enqueue("Content-Type: text/plain; charset=\"iso - 8859 - 1\"");
            results.Enqueue("Content-Transfer-Encoding: quoted-printable");
            results.Enqueue("");
            results.Enqueue("Test");
            results.Enqueue("");
            results.Enqueue("--_002_BLUPR06MB099CF734A1EC6EA5D8185C8B05F0BLUPR06MB099namprd_");
            results.Enqueue("Content-Type: application/x-zip-compressed; name=\"readme.zip\"");
            results.Enqueue("Content-Description: readme.zip");
            results.Enqueue("Content-Disposition: attachment; filename=\"readme.zip\"; size=805;");
            results.Enqueue("	creation-date=\"Thu, 09 Jun 2016 11:56:34 GMT\";");
            results.Enqueue("	modification-date=\"Thu, 09 Jun 2016 11:56:34 GMT\"");
            results.Enqueue("Content-Transfer-Encoding: base64");
            results.Enqueue("");
            results.Enqueue("UEsDBBQAAgAIAAJ7LkhUMsNfrwIAAEAJAAAKAAAAcmVhZG1lLnR4dO2VS2sbMRDHzzb4Owx7WpMg");
            results.Enqueue("AmkvSXMwiUsLeRg7bQ6lB2VXtlV2NUbSxrjB3z0j7Utex6SU9Fbjg3b0n4dmfrua5TzLgKsUjMxX");
            results.Enqueue("mYBMPmquNzBHDVpYLcWTVAvIhTF8IQzMNeYwQWPhbj6XiYCJRosJZvAktJGo4BTiyd3kdAhGaGeD");
            results.Enqueue("tbRLmBeUxxSrFWrrg7Pb8T18YB+Bm41KlhoVFgZWGhea57nPiakoa3uQKsW1gWmhrMwFG/QH/S+4");
            results.Enqueue("BotQGOGeLlEpkVhnmeDqFGY+95nbcs+XmRTKwqpdXoASa2j3YhieD/qtgFURY4hm4+n38TQ6hugb");
            results.Enqueue("LW9HN2O3noxms4e76RWtrS6Edx/0p2XLRN0w6qexvownrtsuXgSlsGuSxAf8jfelfgmeLCH2Bd9U");
            results.Enqueue("oesUUrWBh4P+Mwz6vSB8HTJuHFyqHv3piAYzwR60tOJaKpJEVfCv6Rk8n2zpcJUTazZK99d8r7gV");
            results.Enqueue("XTdnO+zxmWjqejjbYY977Orv8bB6Vjz+oiF2XSqz99uC6/CVNEmFkCfcLkXFrx9A0M5WWc8sZNEN");
            results.Enqueue("IwQ628Cab96RUL7mMpTVnI5c0n8F615Sh2yV8X253ctUB66y/Uf4bxHe62yr35lkl+bOt7em41VQ");
            results.Enqueue("WUBrGS0pT3SNiziqSHWfdvcS0B1R1ccYizpf353K/gxqQqjH6NAqnheK0tBVFA/hmaxAv7AQR6+/");
            results.Enqueue("VxpqKzJMXYl3oeuv0CpsmXs/RirdZdLrt69lr+luq3AXX+zaJ6lrJ8eQCUWLWseM/C3Oae+T26DF");
            results.Enqueue("0VHp2gvex0D/Q/70Te71wtMF+Edw1ACT12Zf745HCX0oTsmyrytRD3UOtH2dAzxUWdzXNFiHQlMa");
            results.Enqueue("mwls/em642vBJZTemFjagTwYVor0fr2FyiXmeaFkwr0oydCItEm5HZ6/AFBLAQIUABQAAgAIAAJ7");
            results.Enqueue("LkhUMsNfrwIAAEAJAAAKAAAAAAAAAAEAIAAAAAAAAAByZWFkbWUudHh0UEsFBgAAAAABAAEAOAAA");
            results.Enqueue("ANcCAAAAAA==");
            results.Enqueue("");
            results.Enqueue("--_002_BLUPR06MB099CF734A1EC6EA5D8185C8B05F0BLUPR06MB099namprd_--");
            results.Enqueue("");

            //results.Enqueue( "+OK" );

            return results;
        }

        #endregion
    }
}