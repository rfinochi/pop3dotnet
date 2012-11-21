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
#if NET45
using System.Threading.Tasks;
#endif

namespace Pop3.TestClient
{
    public class Program
    {
        public static void Main( string[] args )
        {
            const string server = "X";
            const string userName = "Y";
            const string password = "Z";
            const bool useSsl = false;

            Console.ForegroundColor = ConsoleColor.White;

            GetMessages( server, userName, password, useSsl );
#if NET45
            //GetMessagesAsync( server, userName, password, useSsl );
#endif

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

                Collection<Pop3Message> messages = pop3Client.List( );

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
                    Console.WriteLine( );
                }

                Console.WriteLine( "Disconnecting...{0}", Environment.NewLine );
                pop3Client.Disconnect( );
            }
            catch ( Exception ex )
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine( ex.Message );
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

 #if NET45
       public static async Task GetMessagesAsync( string server, string userName, string password, bool useSsl )
        {
            try
            {
                Pop3Client pop3Client = new Pop3Client( );

                Console.WriteLine( "Connecting to POP3 server '{0}'...{1}", server, Environment.NewLine );

                await pop3Client.ConnectAsync( server, userName, password, useSsl );

                Console.WriteLine( "List and Retrieve Messages...{0}", Environment.NewLine );

                Collection<Pop3Message> messages = await pop3Client.ListAndRetrieveAsync( );

                foreach ( Pop3Message message in messages )
                {
                    Console.WriteLine( "- Number: {0}", message.Number );
                    Console.WriteLine( "\t* MessageId: {0}", message.MessageId );
                    Console.WriteLine( "\t* Date: {0}", message.Date );
                    Console.WriteLine( "\t* From: {0}", message.From );
                    Console.WriteLine( "\t* To: {0}", message.To );
                    Console.WriteLine( "\t* Subject: {0}", message.Subject );
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
#endif
    }
}