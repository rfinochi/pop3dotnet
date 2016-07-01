Small and simple library for retrieving messages from Post Office Protocol version 3 (POP3) servers with full support for .NET 4.6, .NET Core 1.0, Windows Runtime and asynchronous programming model.


How to use

Connect to Pop3 Server:

Pop3Client pop3Client = new Pop3Client( );
pop3Client.Connect( "SERVER", "USERNAME", "PASSWORD", true );

Retrieve message list:

var messages = pop3Client.List( );

Retrieve messages:

foreach ( Pop3Message message in messages )
{ 
	pop3Client.Retrieve( message );
	
	Console.WriteLine( "MessageId: {0}", message.MessageId );
	Console.WriteLine( "Date: {0}", message.Date );
	Console.WriteLine( "From: {0}", message.From );
	Console.WriteLine( "To: {0}", message.To );
	Console.WriteLine( "Subject: {0}", message.Subject );
} 

Disconnect from the server:

pop3Client.Disconnect( );


How to use in asynchronously way

Connect to Pop3 Server:

Pop3Client pop3Client = new Pop3Client( );
await pop3Client.ConnectAsync( "SERVER", "USERNAME", "PASSWORD", true );

Retrieve message list:

var messages = await pop3Client.ListAsync( );

Retrieve messages:

foreach ( Pop3Message message in messages )
{ 
	await pop3Client.RetrieveAsync( message );
	
	Console.WriteLine( "MessageId: {0}", message.MessageId );
	Console.WriteLine( "Date: {0}", message.Date );
	Console.WriteLine( "From: {0}", message.From );
	Console.WriteLine( "To: {0}", message.To );
	Console.WriteLine( "Subject: {0}", message.Subject );
} 

Disconnect from the server:

await pop3Client.DisconnectAsync( );


How to use in Windows Runtime

var pop3Client = new Pop3.Pop3Client();

consoleLog("Connecting to POP3 server...");
pop3Client.connectAsync("SERVER", "USERNAME", "PASSWORD", true)
	.then(function () {
	    consoleLog("List and Retrieve Messages...");
	    return pop3Client.listAndRetrieveAsync();
	})
	.then(function (messages) {
	    for (var i = 0, len = messages.size; i < len; i++) {
		var message = messages[i];

		consoleLog("MessageId: " + message.messageId);
		consoleLog("Date: " + message.date);
		consoleLog("From: " + message.from);
		consoleLog("To: " + message.to);
		consoleLog("Subject: " + message.subject);
	    }

	    consoleLog("Disconnecting...");
	    return pop3Client.disconnectAsync();
	})
	.done(function () {
	    consoleLog("Communication closed...");
	});