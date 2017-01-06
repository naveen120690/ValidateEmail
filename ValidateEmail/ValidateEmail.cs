using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ValidateEmail.ImapConnector;
using ValidateEmail.PopConnector;
using ValidateEmail.SmtpConnector;

namespace ValidateEmail
{
    /// <summary>
    /// Email connection security or encryption type
    /// 0- None- non-encrypted message
    /// 1- SSL_TLS- Encypted message
    /// 2- STARTTLS- STARTTLS
    /// </summary>
    public enum NGConnectionSecurity
    {
        None = 0,
        SSL_TLS = 1,
        STARTTLS = 2
    }

    /// <summary>
    /// This class is used to validate email.
    /// </summary>
    /// <Author>Naveen Kumar</Author>
    public class ValidateEmail
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public NGConnectionSecurity ConnectionSecurity { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Protocol { get; set; }
        const string IMAP_OK_SERVER_RESPONSE = "* OK";
        const string POP_OK_SERVER_RESPONSE = "+OK";
        protected const string IMAP_COMMAND_PREFIX = "$";
        protected const string IMAP_CAPABILITY_COMMAND = "CAPABILITY";
        protected const string IMAP_LOGIN_COMMAND = "LOGIN";
        protected const string COMMAND_EOL = "\r\n";
        protected const string IMAP_OK_RESPONSE = IMAP_COMMAND_PREFIX + " " + "OK";
        public bool EnableSsl { get; set; }
        /// <summary>
        /// Constructor to initialize object.
        /// </summary>
        public ValidateEmail()
        {

        }

        /// <summary>
        /// To check authentication of email server
        /// </summary>
        /// <returns>bool</returns>
        public bool Authenticate()
        {
            if (Protocol.StartsWith("IMAP", StringComparison.OrdinalIgnoreCase))
            {

                ImapConnector.ImapConnector connector = new ImapConnector.ImapConnector();
                var isLogin = connector.Login(this.Host, this.Port, this.Username, this.Password, this.ConnectionSecurity);
                connector.Disconnect();
                connector.Dispose();
                return isLogin;
            }
            else if (Protocol.StartsWith("SMTP", StringComparison.OrdinalIgnoreCase))
            {
                SmtpConnector.SmtpConnector connector = new SmtpConnector.SmtpConnector();
                var isLogin=connector.Login(this.Host,this.Port,this.Username,this.Password,this.ConnectionSecurity);
                connector.Disconnect();
                connector.Dispose();
                return isLogin;
            }
            else if (Protocol.StartsWith("POP", StringComparison.OrdinalIgnoreCase))
            {
                PopConnector.PopConnector connector = new PopConnector.PopConnector();
                var isLogin = connector.Login(this.Host, this.Port, this.Username, this.Password, this.ConnectionSecurity);
                connector.Disconnect();
                connector.Dispose();
                return isLogin;
            }
            else
            {
                throw new Exception("Protocol not supported yet");
            }
        }
    }
}
