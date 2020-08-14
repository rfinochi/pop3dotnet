# Small and simple library for retrieving messages from Post Office Protocol version 3 (POP3) servers with full support for .NET 4.8, .NET Standard 2.1 and asynchronous programming model.

[![Build status](https://img.shields.io/appveyor/ci/rfinochi/pop3dotnet.svg?style=plastic)](https://ci.appveyor.com/project/rfinochi/pop3dotnet)
[![Coverage Status](https://img.shields.io/coveralls/github/rfinochi/pop3dotnet?style=plastic)](https://coveralls.io/github/rfinochi/pop3dotnet)
[![CII Best Practices Summary](https://img.shields.io/cii/summary/4203?style=plastic)](https://bestpractices.coreinfrastructure.org/projects/4203)
[![Total alerts](https://img.shields.io/lgtm/alerts/github/rfinochi/pop3dotnet.svg?logo=lgtm&style=plastic)](https://lgtm.com/projects/g/rfinochi/pop3dotnet/alerts/)
[![Issues](https://img.shields.io/github/issues/rfinochi/pop3dotnet?style=plastic)](https://github.com/rfinochi/pop3dotnet/issues)
[![CodeFactor Grade](https://img.shields.io/codefactor/grade/github/rfinochi/pop3dotnet?style=plastic)][(https://www.codefactor.io/repository/github/rfinochi/pop3dotnet/overview/master)
![Code Size](https://img.shields.io/github/languages/code-size/rfinochi/pop3dotnet?style=plastic)
![Latest Release](https://img.shields.io/github/v/release/rfinochi/pop3dotnet?include_prereleases&style=plastic)
[![Latest Version](https://img.shields.io/nuget/v/Pop3.svg?style=plastic)](https://www.nuget.org/packages/Pop3)
[![Downloads](https://img.shields.io/nuget/dt/Pop3.svg?style=plastic)](https://www.nuget.org/packages/Pop3)
[![License](https://img.shields.io/github/license/rfinochi/pop3dotnet.svg?style=plastic)](https://opensource.org/licenses/mit-license.php)

## How to use

Connect to Pop3 Server:

```c#
Pop3Client pop3Client = new Pop3Client( );
pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD", true );
```

Retrieve message list:

```c#
var messages = pop3Client.List( );
```

Retrieve messages:

```c#
foreach ( Pop3Message message in messages )
{ 
	pop3Client.Retrieve( message );
	
	Console.WriteLine( "MessageId: {0}", message.MessageId );
	Console.WriteLine( "Date: {0}", message.Date );
	Console.WriteLine( "From: {0}", message.From );
	Console.WriteLine( "To: {0}", message.To );
	Console.WriteLine( "Subject: {0}", message.Subject );
} 
```

Disconnect from the server:

```c#
pop3Client.Disconnect( );
```

#### How to use in asynchronously way

Connect to Pop3 Server:

```c#
Pop3Client pop3Client = new Pop3Client( );
await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD", true );
```

Retrieve message list:

```c#
var messages = await pop3Client.ListAsync( );
```

Retrieve messages:

```c#
foreach ( Pop3Message message in messages )
{ 
	await pop3Client.RetrieveAsync( message );
	
	Console.WriteLine( "MessageId: {0}", message.MessageId );
	Console.WriteLine( "Date: {0}", message.Date );
	Console.WriteLine( "From: {0}", message.From );
	Console.WriteLine( "To: {0}", message.To );
	Console.WriteLine( "Subject: {0}", message.Subject );
} 
```

Disconnect from the server:

```c#
await pop3Client.DisconnectAsync( );
```
