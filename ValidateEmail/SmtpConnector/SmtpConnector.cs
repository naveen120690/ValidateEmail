using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ValidateEmail.SmtpConnector
{
    /// <summary>
    /// Server connector for SMTP protocol. Secure connection to server.
    /// </summary>
    /// <Author>Naveen Kumar</Author>
    public class SmtpConnector
    {
        private TcpClient _Client;
        private NetworkStream _ClientStream;
        private StreamReader _StreamReader;
        private StreamWriter _StreamWriter;
        private SslStream _SslStream;
        private StreamReader _SslStreamReader;
        private StreamWriter _SslStreamWriter;
        private bool _bSsl;
        private const string EOF = "\r\n";

        /// <summary>
        /// SMTP login
        /// </summary>
        /// <param name="host">Host of email server</param>
        /// <param name="port">Port of server</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="connectionSecurity">Connection security/Encryption Type</param>
        /// <returns>bool value</returns>
        public bool Login(string host, int port, string username, string password, NGConnectionSecurity connectionSecurity)
        {
            try
            {
                Logger.Trace("Connecting to " + host + ":" + port + " with username:" + username + " and secutity:" + connectionSecurity.ToString());
                username = Convert.ToBase64String(Encoding.UTF8.GetBytes(username));
                password = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

                _bSsl = false;
                _Client = new TcpClient(host, port);
                _ClientStream = _Client.GetStream();
                _StreamReader = new StreamReader(_ClientStream);
                _StreamWriter = new StreamWriter(_ClientStream);
                _SslStream = new SslStream(_ClientStream);

                if (connectionSecurity == NGConnectionSecurity.SSL_TLS)
                {
                    _SslStream.AuthenticateAsClient(host);
                    _bSsl = true;
                    _SslStreamReader = new StreamReader(_SslStream);
                    _SslStreamWriter = new StreamWriter(_SslStream);
                }


                var response = this.Read();
                if (!response.StartsWith(NGSmtpStatusCode.ServiceReady.ToStringValue()))
                {
                    Dispose();
                    return false;
                }


                this.Write(string.Format(NGSmtpCommands.HELO_CMD, Dns.GetHostName(), EOF));
                response = this.Read();
                if (!response.StartsWith(NGSmtpStatusCode.Ok.ToStringValue()))
                {
                    Dispose();
                    return false;
                }


                if (connectionSecurity == NGConnectionSecurity.STARTTLS)
                {
                    this.Write(string.Format(NGSmtpCommands.STARTTLS_CMD, EOF));
                    response = this.Read();
                    if (!response.StartsWith(NGSmtpStatusCode.ServiceReady.ToStringValue()))
                    {
                        Dispose();
                        return false;
                    }

                    _SslStream.AuthenticateAsClient(host);
                    _bSsl = true;
                    _SslStreamReader = new StreamReader(_SslStream);
                    _SslStreamWriter = new StreamWriter(_SslStream);

                    this.Write(string.Format(NGSmtpCommands.EHLO_CMD, Dns.GetHostName(), EOF));
                    response = this.Read();
                    if (!response.StartsWith(NGSmtpStatusCode.Ok.ToStringValue()))
                    {
                        Dispose();
                        return false;
                    }
                }

                this.Write(string.Format(NGSmtpCommands.AUTH_LOGIN_CMD, EOF));
                response = this.Read();
                if (!response.StartsWith(NGSmtpStatusCode.NextAcceptBase64.ToStringValue()))
                {
                    Dispose();
                    return false;
                }

                this.Write(string.Format("{0}{1}", username, EOF));
                response = this.Read();
                if (!response.StartsWith(NGSmtpStatusCode.NextAcceptBase64.ToStringValue()))
                {
                    Dispose();
                    return false;
                }

                this.Write(string.Format("{0}{1}", password, EOF));
                response = this.Read();
                if (!response.StartsWith(NGSmtpStatusCode.Authenticated.ToStringValue()))
                {
                    Dispose();
                    return false;
                }
                Logger.Trace("Connection to " + host + ":" + port + " successful");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("Error in Login", ex);
                return false;
            }
        }

        /// <summary>
        /// Writes text command to stream.
        /// </summary>
        /// <param name="text">Text Command</param>
        private void Write(string text)
        {
            Logger.Trace("Request command :" + text);
            try
            {
                if (_bSsl)
                {
                    _SslStreamWriter.Write(text);
                    _SslStreamWriter.Flush();
                }
                else
                {
                    _StreamWriter.Write(text);
                    _StreamWriter.Flush();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error in writing to server", ex);
            }
        }

        /// <summary>
        /// Read line from stream and return as string
        /// </summary>
        /// <returns>String</returns>
        private string Read()
        {
            var response = string.Empty;
            try
            {
                if (_bSsl)
                {
                    byte[] buffer = new byte[2048];
                    StringBuilder messageData = new StringBuilder();
                    int bytes = -1;
                    do
                    {
                        bytes = _SslStream.Read(buffer, 0, buffer.Length);

                        // Use Decoder class to convert from bytes to UTF8
                        // in case a character spans two buffers.
                        Decoder decoder = Encoding.UTF8.GetDecoder();
                        char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                        decoder.GetChars(buffer, 0, bytes, chars, 0);
                        messageData.Append(chars);
                        // Check for EOF.
                        if (messageData.ToString().IndexOf(EOF) != -1)
                        {
                            break;
                        }
                    } while (bytes != 0);
                    response = messageData.ToString();
                    Logger.Trace("Response from server:" + response);
                    return response;
                }
                else
                {
                    response = _StreamReader.ReadLine();
                    Logger.Trace("Response from server:" + response);
                    return response;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error in reading from server", ex);
                return Convert.ToString(NGSmtpStatusCode.ServiceNotAvailable);
            }
        }

        /// <summary>
        /// Dispose all stream and connection.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (_SslStreamWriter != null)
                {
                    _SslStreamWriter.Close();
                }
                if (_SslStreamReader != null)
                {
                    _SslStreamReader.Close();
                }
                if (_SslStream != null)
                {
                    _SslStream.Close();
                }
                if (_StreamWriter != null)
                {
                    _StreamWriter.Close();
                }
                if (_StreamReader != null)
                {
                    _StreamReader.Close();
                }
                if (_ClientStream != null)
                {
                    _ClientStream.Close();
                }
                if (_Client != null)
                {
                    _Client.Close();
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Disconnect client from server.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                this.Write(string.Format(NGSmtpCommands.QUIT_CMD, EOF));
                var response = this.Read();
                if (!response.StartsWith(NGSmtpStatusCode.ServiceClosingTransmissionChannel.ToStringValue()))
                    Logger.Trace("Problem in closing connection");
            }
            catch (Exception)
            {

            }
            
        }
    }
}
