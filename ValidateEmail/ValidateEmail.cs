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
    /// This class is used to validate email.
    /// </summary>
    /// <Author>Naveen Kumar</Author>
    public class ValidateEmail
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
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

        /// <summary>
        /// Constructor to initialize object.
        /// </summary>
        public ValidateEmail()
        {

        }

        /// <summary>
        /// To check authentication of email server
        /// </summary>
        /// <returns></returns>
        public bool Authenticate()
        {
            if (Protocol.StartsWith("IMAP", StringComparison.OrdinalIgnoreCase))
            {
                ImapConnectorBase connector;
                if (EnableSsl)
                {
                    connector = new ImapConnectorWithSsl(Host, Port);
                }
                else
                {
                    connector = new ImapConnectorWithoutSsl(Host, Port);
                }
                if (!connector.CheckResponse(IMAP_OK_SERVER_RESPONSE))
                {
                    connector.Dispose();
                    return false;
                }
                connector.SendData(IMAP_COMMAND_PREFIX + " " + IMAP_CAPABILITY_COMMAND + COMMAND_EOL);
                if (!connector.CheckResponse(IMAP_OK_RESPONSE))
                {
                    connector.Dispose();
                    return false;
                }
                connector.SendData(IMAP_COMMAND_PREFIX + " " + IMAP_LOGIN_COMMAND + " " + Username + " " + Password + COMMAND_EOL);
                if (!connector.CheckResponse(IMAP_OK_RESPONSE))
                {
                    connector.Dispose();
                    return false;
                }
                return true;
            }
            else if (Protocol.StartsWith("SMTP", StringComparison.OrdinalIgnoreCase))
            {
                SmtpConnectorBase connector;
                if (EnableSsl)
                {
                    connector = new SmtpConnectorWithSsl(Host, Port);
                }
                else
                {
                    connector = new SmtpConnectorWithoutSsl(Host, Port);
                }

                if (!connector.CheckResponse(220))
                {
                    connector.Dispose();
                    return false;
                }

                connector.SendData(string.Format("HELO {0}{1}", Dns.GetHostName(), SmtpConnectorBase.EOF));
                if (!connector.CheckResponse(250))
                {
                    connector.Dispose();
                    return false;
                }

                connector.SendData("AUTH LOGIN" + SmtpConnectorBase.EOF);
                if (!connector.CheckResponse(334))
                {
                    connector.Dispose();
                    return false;
                }

                connector.SendData(Convert.ToBase64String(Encoding.UTF8.GetBytes(Username)) + SmtpConnectorBase.EOF);
                if (!connector.CheckResponse(334))
                {
                    connector.Dispose();
                    return false;
                }

                connector.SendData(Convert.ToBase64String(Encoding.UTF8.GetBytes(Password)) + SmtpConnectorBase.EOF);
                if (!connector.CheckResponse(235))
                {
                    connector.Dispose();
                    return false;
                }

                return true;
            }
            else if (Protocol.StartsWith("POP", StringComparison.OrdinalIgnoreCase))
            {
                PopConnectorBase connector;
                if (EnableSsl)
                {
                    connector = new PopConnectorWithSsl(Host, Port);
                }
                else
                {
                    connector = new PopConnectorWithoutSsl(Host, Port);
                }
                if (!connector.CheckResponse(POP_OK_SERVER_RESPONSE))
                {
                    connector.Dispose();
                    return false;
                }
                //connector.SendData(IMAP_COMMAND_PREFIX + " " + IMAP_CAPABILITY_COMMAND + IMAP_COMMAND_EOL);
                //if (!connector.CheckResponse(IMAP_OK_RESPONSE))
                //{
                //     connector.Dispose();
                //   return false;
                //}
                connector.SendData("USER " + Username + COMMAND_EOL);
                if (!connector.CheckResponse(POP_OK_SERVER_RESPONSE))
                {
                    connector.Dispose();
                    return false;
                }
                connector.SendData("PASS " + Password + COMMAND_EOL);
                if (!connector.CheckResponse(POP_OK_SERVER_RESPONSE))
                {
                    connector.Dispose();
                    return false;
                }
                return true;
            }
            else
            {
                throw new Exception("Protocol not supported yet");
            }
        }
    }
}
