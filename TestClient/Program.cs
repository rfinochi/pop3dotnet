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

namespace Pop3.TestClient
{
    public class Program
    {
        public static void Main( string[] args )
        {
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine( "Enter Server:" );
            string server = Console.ReadLine( );

            Console.WriteLine( "Enter User Name:" );
            string userName = Console.ReadLine( );

            Console.WriteLine( "Enter Password:" );
            string password = Console.ReadLine( );

            Console.WriteLine( "Use SSL (1 or 0):" );
            bool useSsl = ( Console.ReadLine( ) == "1" ? true : false );

            GetMessages( server, userName, password, useSsl );
            GetMessagesAsync( server, userName, password, useSsl ).Wait( );

            Console.WriteLine( "Press any key to close", Environment.NewLine );
            Console.ReadLine( );
        }

        public static void GetMessages( string server, string userName, string password, bool useSsl )
        {
            try
            {
                Pop3Client pop3Client = new Pop3Client( );

                Console.WriteLine( "Connecting to POP3 server '{0}'...{1}", server, Environment.NewLine );

                pop3Client.Connect( server, userName, password, useSsl );

                Console.WriteLine( "List Messages...{0}", Environment.NewLine );

                IEnumerable<Pop3Message> messages = pop3Client.List( );

                Console.WriteLine( "Retrieve Messages...{0}", Environment.NewLine );

                foreach ( Pop3Message message in messages )
                {
                    Console.WriteLine( "- Number: {0}", message.Number );

                    pop3Client.Retrieve( message );

                    Console.WriteLine( "\t* MessageId: {0}", message.MessageId );
                    Console.WriteLine( "\t* Date: {0}", message.Date );
                    Console.WriteLine( "\t* From: {0}", message.From );
                    Console.WriteLine( "\t* To: {0}", message.To );
                    Console.WriteLine( "\t* Subject: {0}", message.Subject );
                    Console.WriteLine( "\t* Body Length: {0}", message.Body.Length );
                    Console.WriteLine( );
                }

                Console.WriteLine( "Disconnecting...{0}", Environment.NewLine );
                pop3Client.Disconnect( );
        }
            catch (Exception ex )
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message );
                Console.ForegroundColor = ConsoleColor.White;
            }
}

        public static async Task GetMessagesAsync( string server, string userName, string password, bool useSsl )
        {
            try
            {
                Pop3Client pop3Client = new Pop3Client( );

                Console.WriteLine( "Connecting to POP3 server '{0}'...{1}", server, Environment.NewLine );

                await pop3Client.ConnectAsync( server, userName, password, useSsl );

                Console.WriteLine( "List and Retrieve Messages...{0}", Environment.NewLine );

                IEnumerable<Pop3Message> messages = await pop3Client.ListAndRetrieveAsync( );

                foreach ( Pop3Message message in messages )
                {
                    Console.WriteLine( "- Number: {0}", message.Number );
                    Console.WriteLine( "\t* MessageId: {0}", message.MessageId );
                    Console.WriteLine( "\t* Date: {0}", message.Date );
                    Console.WriteLine( "\t* From: {0}", message.From );
                    Console.WriteLine( "\t* To: {0}", message.To );
                    Console.WriteLine( "\t* Subject: {0}", message.Subject );
                    Console.WriteLine( "\t* Body Length: {0}", message.Body.Length );
                    Console.WriteLine( );
                }

                Console.WriteLine( "Disconnecting...{0}", Environment.NewLine );
                await pop3Client.DisconnectAsync( );
            }
            catch ( Exception ex )
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine( ex.Message );
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}