/*
 * Author: Rodolfo Finochietti
 * Email: rfinochi@shockbyte.net
 * Web: http://shockbyte.net
 *
 * This work is licensed under the Creative Commons Attribution License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by/2.0/ 
 * or send a letter to Creative Commons, 559 Nathan Abbott Way, Stanford, California 94305, USA.
 * 
 * No warranties expressed or implied, use at your own risk.
 */
using System;
using System.Collections.Generic;

using Pop3;

namespace TestClient
{
    class Program
    {
        static void Main( string[] args )
        {
            string server = "XXX";
            string userName = "XXX";
            string password = "XXX";
            
            Pop3Client popClient = new Pop3Client( );

            Console.WriteLine( "Connecting to POP3 server '{0}'...{1}", server, Environment.NewLine );

            popClient.Connect( server, userName, password, true );

			Console.WriteLine( "List Messages...{0}", Environment.NewLine );
			List<Pop3Message> messages = popClient.List( );

			Console.WriteLine( "Retrieve Messages...{0}", Environment.NewLine );
			foreach ( Pop3Message message in messages )
			{
				Console.WriteLine( "- Number: {0}", message.Number );

				//popClient.RetrieveHeader( message );
				//Console.WriteLine( "\t* Header: {0}", message.Header );
	
				popClient.Retrieve( message );
				Console.WriteLine( "\t* MessageID: {0}", message.MessageId );
				Console.WriteLine( "\t* Date: {0}", message.Date );
				Console.WriteLine( "\t* From: {0}", message.From );
				Console.WriteLine( "\t* To: {0}", message.To );
				Console.WriteLine( "\t* Subject: {0}", message.Subject );

				Console.WriteLine( );
			}

            Console.WriteLine( "Disconnecting...{0}", Environment.NewLine );
            popClient.Disconnect( );

            Console.ReadLine( );
        }
    }
}