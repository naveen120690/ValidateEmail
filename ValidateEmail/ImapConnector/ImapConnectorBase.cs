using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateEmail.ImapConnector
{
    /// <summary>
    /// Connector Base class  for IMAP protocol
    /// </summary>
    /// <Author>Naveen Kumar</Author>
    internal abstract class ImapConnectorBase
    {
        /// <summary>
        /// Email Server address for Imap
        /// </summary>
        protected string ImapServerAddress { get; set; }
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
        /// <param name="imapServerAddress">Server Address</param>
        /// <param name="port">Server Port</param>
        protected ImapConnectorBase(string imapServerAddress, int port)
        {
            ImapServerAddress = imapServerAddress;
            Port = port;
        }

        /// <summary>
        /// Check Response Method
        /// </summary>
        /// <param name="expectedCode">Expected code</param>
        /// <returns>boolean</returns>
        public abstract bool CheckResponse(string expectedCode);
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
