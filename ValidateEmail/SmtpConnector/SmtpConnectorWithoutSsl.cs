using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ValidateEmail.SmtpConnector
{
    /// <summary>
    /// Server connector for SMTP protocol. Non secure connection to server
    /// </summary>
    /// <Author>Naveen Kumar</Author>
    internal class SmtpConnectorWithoutSsl : SmtpConnectorBase
    {
        private Socket _socket = null;
        private const int MAX_ATTEMPTS_COUNT = 100;

        /// <summary>
        /// Constructor to initialize parameters
        /// </summary>
        /// <param name="smtpServerAddress">Server Address</param>
        /// <param name="port">Server Port</param>
        public SmtpConnectorWithoutSsl(string smtpServerAddress, int port)
            : base(smtpServerAddress, port)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(smtpServerAddress);
                IPEndPoint endPoint = new IPEndPoint(hostEntry.AddressList[0], port);
                _socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                //try to connect and test the rsponse for code 220 = success
                _socket.Connect(endPoint);
            }
            catch (Exception)
            {
                _socket = null;
            }
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~SmtpConnectorWithoutSsl()
        {
            try
            {
                if (_socket != null)
                {
                    _socket.Close();
                    _socket.Dispose();
                    _socket = null;
                }
            }
            catch (Exception)
            {
                ;
            }
        }

        /// <summary>
        /// To check response from last request with expected code
        /// </summary>
        /// <param name="expectedCode">Expected output from the server</param>
        /// <returns>boolean</returns>
        public override bool CheckResponse(int expectedCode)
        {
            if (_socket == null)
            {
                return false;
            }
            var currentAttemptIndex = 1;
            while (_socket.Available == 0)
            {
                System.Threading.Thread.Sleep(100);
                if (currentAttemptIndex++ > MAX_ATTEMPTS_COUNT)
                {
                    return false;
                }
            }
            byte[] responseArray = new byte[9000];
            _socket.Receive(responseArray, 0, _socket.Available, SocketFlags.None);
            string responseData = Encoding.UTF8.GetString(responseArray);
            int responseCode = Convert.ToInt32(responseData.Substring(0, 3));
            if (responseCode == expectedCode)
            {
                return true;
            }
            return false;
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
            byte[] dataArray = Encoding.UTF8.GetBytes(data);
            _socket.Send(dataArray, 0, dataArray.Length, SocketFlags.None);
        }
        
        /// <summary>
        /// To close the current server connection.
        /// </summary>
        public override void Dispose()
        {
            if (_socket != null)
            {
                _socket.Close();
            }
        }
    }
}
