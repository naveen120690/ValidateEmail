using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ValidateEmail.ImapConnector
{
    /// <summary>
    /// Server connector for Imap protocol. Secure connection to server.
    /// </summary>
    /// <Author>Naveen Kumar</Author>
    class ImapConnectorWithSsl : ImapConnectorBase
    {
        private TcpClient _socket = null;
        private Stream _stream=null;
        private StreamReader _streamReader = null;
        private const int MAX_ATTEMPTS_COUNT = 100;
        private const int RECEIVE_TIMEOUT = 60000;
        private const int SEND_TIMEOUT = 60000;
        protected const string IMAP_NO_RESPONSE = "$ NO";
        protected const string IMAP_BAD_RESPONSE = "$ BAD";

        /// <summary>
        /// Constructor to initialize parameters
        /// </summary>
        /// <param name="imapServerAddress">Server Address</param>
        /// <param name="port">Server Port</param>
        public ImapConnectorWithSsl(string imapServerAddress, int port) :base(imapServerAddress,port)
        {
            try
            {
                _socket = new TcpClient();
                _socket.ReceiveTimeout = RECEIVE_TIMEOUT;
                _socket.SendTimeout = SEND_TIMEOUT;
                _socket.Connect(imapServerAddress, port);
                SslStream objSSLStream;
                objSSLStream = new SslStream(_socket.GetStream(), false);
                objSSLStream.ReadTimeout = RECEIVE_TIMEOUT;
                objSSLStream.WriteTimeout = SEND_TIMEOUT;

                // Authenticate the server
                objSSLStream.AuthenticateAsClient(imapServerAddress);

                _stream = objSSLStream;
                _streamReader = new StreamReader(_stream);
            }
            catch (Exception)
            {
                if (_socket != null)
                {
                    _socket.Close();
                    _socket = null;
                }
                
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~ImapConnectorWithSsl()
        {
            try {
                if (_socket != null) {
                    _socket.Close();
                    _socket = null;
                }
            } catch (Exception) {
                ;
            }
        }

        /// <summary>
        /// To check response from last request with expected code
        /// </summary>
        /// <param name="expectedCode">Expected output from the server</param>
        /// <returns>boolean</returns>
        public override bool CheckResponse(string expectedCode)
        {
            if (_socket == null)
            {
                return false;
            }
            string strServerResponse = string.Empty;
            bool bResponse = false;
            bool bRead = true;
            while (bRead)
            {
                strServerResponse = _streamReader.ReadLine();
                Console.WriteLine("Mail Server-Response: " + strServerResponse);
                if (strServerResponse == null)
                {
                    return false;
                }
                if (strServerResponse.StartsWith(expectedCode))
                {
                    bRead = false;
                    bResponse = true;
                }
                else if (strServerResponse.StartsWith(IMAP_NO_RESPONSE))
                {
                    bRead = false;
                }
                else if (strServerResponse.StartsWith(IMAP_BAD_RESPONSE))
                {
                    bRead = false;
                }
                else
                {
                    bRead = true;
                }

            }
            return bResponse;
        }

        /// <summary>
        /// To send data to server
        /// </summary>
        /// <param name="data">data to be send</param>
        public override void SendData(string data)
        {
            if (_socket == null)
            {
                return;
            }
            try
            {
                // Convert the command with CRLF afterwards as per RFC to a byte array which we can write
                byte[] commandBytes = Encoding.ASCII.GetBytes(data.ToCharArray());
                // Write the command to the server
                _stream.Write(commandBytes, 0, commandBytes.Length);
                _stream.Flush();
            }
            catch (Exception)
            {
                ;
            }
        }

        /// <summary>
        /// To close the current server connection.
        /// </summary>
        public override void Dispose()
        {
            if (_stream != null)
            {
                _stream.Close();
            }
            DisconnectStreams();
            _socket.Close();
        }

        /// <summary>
        /// To Close all opened streams.
        /// </summary>
        private void DisconnectStreams()
        {
            try
            {
                if (_stream != null)
                {
                    _stream.Close();
                }
            }
            catch (Exception)
            {
                ;
            }
        }
    }
}
