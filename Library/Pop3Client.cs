/*
 * Author: Rodolfo Finochietti
 * Email: rfinochi@shockbyte.net
 * Web: http://shockbyte.net
 *
 * This work is licensed under the Creative Commons Attribution License. 
 * To view a copy of this license, visit  http://creativecommons.org/licenses/by/2.0
 * or send a letter to Creative Commons, 559 Nathan Abbott Way, Stanford, California 94305, USA.
 * 
 * No warranties expressed or implied, use at your own risk.
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Net.Security;
using System.IO;

namespace Pop3
{
    public class Pop3Client : TcpClient
    {
        #region Private Fields

        private string _server;
        private int _portNumber = 995;
        private bool _useSsl = true;
        private bool _isConnected;

        private Stream _pop3Stream;

        #endregion


        #region Public Methods

        public void Connect(string server, string userName, string password)
        {
            Connect(server, userName, password, 110, false);
        }

        public void Connect(string server, string userName, string password, bool useSsl)
        {
            Connect(server, userName, password, (useSsl ? 995 : 110), useSsl);
        }

        public void Connect(string server, string userName, string password, int portNumber, bool useSsl)
        {
            if (_isConnected)
                throw new Pop3Exception("Pop3 client already connected");

            _server = server;
            _useSsl = useSsl;
            _portNumber = portNumber;

            Connect(server, _portNumber);

            if (_useSsl)
            {
                _pop3Stream = new SslStream(GetStream(), false);
                ((SslStream)_pop3Stream).AuthenticateAsClient(_server);
            }
            else
            {
                _pop3Stream = GetStream();
            }

            var response = Response();
            if (response.Substring(0, 3) != "+OK")
            {
                throw new Pop3Exception(response);
            }

            SendCommand(String.Format(CultureInfo.InvariantCulture, "USER {0}", userName));
            SendCommand(String.Format(CultureInfo.InvariantCulture, "PASS {0}", password));

            _isConnected = true;
        }

        public void Disconnect()
        {
            if (!_isConnected)
                return;

            try
            {
                SendCommand("QUIT");
            }
            finally
            {
                _isConnected = false;
            }
        }

        public List<Pop3Message> List()
        {
            var result = new List<Pop3Message>();

            SendCommand("LIST");

            while (true)
            {
                var response = Response();
                if (response == ".\r\n")
                    return result;


                var message = new Pop3Message();

                char[] seps = { ' ' };
                var values = response.Split(seps);

                message.Number = Int32.Parse(values[0]);
                message.Bytes = Int32.Parse(values[1]);
                message.Retrieved = false;

                result.Add(message);
            }
        }

        public void RetrieveHeader(Pop3Message message)
        {
            SendCommand("TOP", "0", message);

            while (true)
            {
                var response = Response();
                if (response == ".\r\n")
                    break;

                message.Header += response;
            }
        }

        public void Retrieve(Pop3Message message)
        {
            SendCommand("RETR", message);

            message.Retrieved = true;
            while (true)
            {
                var response = Response();
                if (response == ".\r\n")
                    break;

                message.Message += response;
            }
        }

        public void Retrieve(List<Pop3Message> messages)
        {
            foreach (var message in messages)
            {
                Retrieve(message);
            }
        }

        public List<Pop3Message> ListAndRetrieve()
        {
            var messages = List();

            Retrieve(messages);

            return messages;
        }

        public void Delete(Pop3Message message)
        {
            SendCommand("DELE", message);
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion


        #region Private Methods

        protected override void Dispose(bool disposing)
        {
            Disconnect();

            base.Dispose(disposing);
        }

        private void SendCommand(string command, Pop3Message message)
        {
            SendCommand(command, null, message);
        }

        private void SendCommand(string command, string aditionalParameters = null, Pop3Message message = null)
        {
            var request = new StringBuilder();

            if (message == null)
            {
                request.AppendFormat(CultureInfo.InvariantCulture, "{0}", command);
            }
            else
            {
                request.AppendFormat(CultureInfo.InvariantCulture, "{0} {1}", command, message.Number);
            }

            if (!String.IsNullOrEmpty(aditionalParameters))
            {
                request.AppendFormat(" {0}", aditionalParameters);
            }

            request.Append("\r\n");

            Write(request.ToString());

            var response = Response();

            if (response.Substring(0, 3) != "+OK")
                throw new Pop3Exception(response);
        }

        private void Write(string message)
        {
            var en = new ASCIIEncoding();

            var writeBuffer = en.GetBytes(message);

            _pop3Stream.Write(writeBuffer, 0, writeBuffer.Length);
        }

        private string Response()
        {
            var enc = new ASCIIEncoding();

            var serverBuffer = new Byte[1024];

            var count = 0;

            while (true)
            {
                var buff = new Byte[2];
                var bytes = _pop3Stream.Read(buff, 0, 1);
                if (bytes != 1)
                {
                    break;
                }
                
                serverBuffer[count] = buff[0];
                count++;

                if (buff[0] == '\n')
                    break;
            }

            return enc.GetString(serverBuffer, 0, count);
        }

        #endregion
    }
}