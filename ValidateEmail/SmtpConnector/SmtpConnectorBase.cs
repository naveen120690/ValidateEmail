using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateEmail.SmtpConnector
{
    /// <summary>
    /// Connector Base class  for POP protocol
    /// </summary>
    /// <Author>Naveen Kumar</Author>
    internal abstract class SmtpConnectorBase
    {
        /// <summary>
        /// Email Server address for Imap
        /// </summary>
        protected string SmtpServerAddress { get; set; }
        /// <summary>
        /// Email Server Port
        /// </summary>
        protected int Port { get; set; }
        /// <summary>
        /// End of command 
        /// </summary>
        public const string EOF = "\r\n";

        /// <summary>
        /// Constructor to initilize parameters
        /// </summary>
        /// <param name="popServerAddress">Server Address</param>
        /// <param name="port">Server Port</param>
        protected SmtpConnectorBase(string smtpServerAddress, int port)
        {
            SmtpServerAddress = smtpServerAddress;
            Port = port;
        }

        /// <summary>
        /// Check Response Method
        /// </summary>
        /// <param name="expectedCode">Expected code</param>
        /// <returns>int</returns>
        public abstract bool CheckResponse(int expectedCode);
        /// <summary>
        /// To send data to email server
        /// </summary>
        /// <param name="data">Data as string</param>
        public abstract void SendData(string data);
        /// <summary>
        /// To dispose the current connection to server.
        /// </summary>
        public abstract void Dispose();
    }
}
