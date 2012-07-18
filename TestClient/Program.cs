/*
 * Author: Rodolfo Finochietti
 * Email: rfinochi@shockbyte.net
 * Web: http://shockbyte.net
 *
 * This work is licensed under the Creative Commons Attribution License. 
 * To view a copy of this license, visit   http://creativecommons.org/licenses/by/2.0
 * or send a letter to Creative Commons, 559 Nathan Abbott Way, Stanford, California 94305, USA.
 * 
 * No warranties expressed or implied, use at your own risk.
 */
using System;
using System.Collections.Generic;

namespace TestClient
{
    using Pop3;

    public class Program
    {
        public static void Main(string[] args)
        {
            const string server = "XXXX";
            const string userName = "XXXX";
            const string password = "XXXX";

            var pop3Client = new Pop3Client();

            Console.WriteLine("Connecting to POP3 server '{0}'...{1}", server, Environment.NewLine);

            pop3Client.Connect(server, userName, password, true);

            Console.WriteLine("List Messages...{0}", Environment.NewLine);

            var messages = pop3Client.List();

            Console.WriteLine("Retrieve Messages...{0}", Environment.NewLine);

            foreach (var message in messages)
            {
                Console.WriteLine("- Number: {0}", message.Number);

                pop3Client.Retrieve(message);

                Console.WriteLine("\t* MessageID: {0}", message.MessageId);
                Console.WriteLine("\t* Date: {0}", message.Date);
                Console.WriteLine("\t* From: {0}", message.From);
                Console.WriteLine("\t* To: {0}", message.To);
                Console.WriteLine("\t* Subject: {0}", message.Subject);
                Console.WriteLine();
            }

            Console.WriteLine("Disconnecting...{0}", Environment.NewLine);
            pop3Client.Disconnect();

            Console.ReadLine();
        }
    }
}