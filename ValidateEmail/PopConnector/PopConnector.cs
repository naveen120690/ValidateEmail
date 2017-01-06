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

namespace ValidateEmail.PopConnector
{
    /// <summary>
    /// Server connector for SMTP protocol. Secure connection to server.
    /// </summary>
    /// <Author>Naveen Kumar</Author>
    public class PopConnector
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
        /// POP login
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

                if (!this.CheckResponse(NGPopStatusCode.POP_OK))
                {
                    Dispose();
                    return false;
                }

                if (connectionSecurity == NGConnectionSecurity.STARTTLS)
                {
                    this.Write(string.Format(NGPopCommands.STARTTLS_CMD, NGPopCommands.EOL));
                    if (!this.CheckResponse(NGPopStatusCode.POP_OK))
                    {
                        Dispose();
                        return false;
                    }

                    _SslStream.AuthenticateAsClient(host);
                    _bSsl = true;
                    _SslStreamReader = new StreamReader(_SslStream);
                    _SslStreamWriter = new StreamWriter(_SslStream);

                }

                this.Write(string.Format(NGPopCommands.LOGIN_USER_CMD, username,NGPopCommands.EOL));
                if (!this.CheckResponse(NGPopStatusCode.POP_OK))
                {
                    Dispose();
                    return false;
                }
                this.Write(string.Format(NGPopCommands.LOGIN_PASS_CMD, password, NGPopCommands.EOL));
                if (!this.CheckResponse(NGPopStatusCode.POP_OK))
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
        /// Check Response from server and return true if matched with expected response
        /// </summary>
        /// <param name="expectedCode">Expected code</param>
        /// <returns>bool</returns>
        private bool CheckResponse(string expectedCode)
        {
            var response = false;
            try
            {
                string strServerResponse = string.Empty;
                response = false;
                bool bRead = true;
                while (bRead)
                {
                    if (_bSsl)
                    {
                        strServerResponse = _SslStreamReader.ReadLine();
                    }
                    else
                    {
                        strServerResponse = _StreamReader.ReadLine();
                    }

                    Logger.Trace("Response from server:" + strServerResponse);
                    if (strServerResponse == null)
                    {
                        return false;
                    }
                    if (strServerResponse.StartsWith(expectedCode))
                    {
                        bRead = false;
                        response = true;
                    }
                    else if (strServerResponse.StartsWith(NGPopStatusCode.POP_NO))
                    {
                        bRead = false;
                    }
                    else if (strServerResponse.StartsWith(NGPopStatusCode.POP_BAD))
                    {
                        bRead = false;
                    }
                    else
                    {
                        bRead = true;
                    }
                }
                return response;
            }
            catch (Exception ex)
            {
                Logger.Error("Error in reading from server", ex);
                return response;
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
                //this.Write(string.Format(NGPopStatusCode.QUIT_CMD, EOF));
                //var response = this.Read();
                //if (!response.StartsWith(NGSmtpStatusCode.ServiceClosingTransmissionChannel.ToStringValue()))
                //    Logger.Trace("Problem in closing connection");
            }
            catch (Exception)
            {

            }
            
        }
    }
}
