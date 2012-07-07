POP3.NET is a simple and small library for retrieving messages from Post Office Protocol version 3 (POP3) servers.

How to use

- Connect to Pop3 Server:
	Pop3Client pop3Client = new Pop3Client( );
	pop3Client.Connect( server, userName, password, true );

- Retrive message list:
	List<Pop3Message> messages = pop3Client.List( );

-Retrive messages:
	foreach ( Pop3Message message in messages )
	{ 
		pop3Client.Retrieve( message );
	
		Console.WriteLine( "\t* MessageID: {0}", message.MessageId );
		Console.WriteLine( "\t* Date: {0}", message.Date );
		Console.WriteLine( "\t* From: {0}", message.From );
		Console.WriteLine( "\t* To: {0}", message.To );
		Console.WriteLine( "\t* Subject: {0}", message.Subject );
	} 

- Disconnect from the server:
	pop3Client.Disconnect( );