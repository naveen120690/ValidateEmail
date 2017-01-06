using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateEmail.SmtpConnector
{
    /// <summary>
    /// It contains Smtp Commands used in communication with smtp email server.
    /// </summary>
    public static class NGSmtpCommands
    {
        public static string HELO_CMD = "HELO {0}{1}";
        public static string STARTTLS_CMD = "STARTTLS{0}";
        public static string EHLO_CMD = "EHLO {0}{1}";
        public static string AUTH_LOGIN_CMD = "AUTH LOGIN{0}";
        public static string MAIL_FROM_CMD = "MAIL From: <{0}>{1}";
        public static string RCPT_TO_CMD = "rcpt to: <{0}>{1}";
        public static string DATA_CMD = "data{0}";
        public static string FROM_CMD = "From: {0}{1}";
        public static string TO_CMD = "To: {0}{1}";
        public static string CC_CMD = "Cc: {0}{1}";
        public static string BCC_CMD = "Bcc: {0}{1}";
        public static string REPLY_TO_CMD = "Reply-To: {0}{1}";
        public static string SUBJECT_CMD = "Subject: {0}{1}";
        public static string MIME_VERSION_CMD = "Mime-Version: 1.0{0}";
        public static string CONTENT_TYPE_BOUNDARY_CMD = "Content-Type: {0}; boundary=\"{1}\"";
        public static string BOUNDARY = "boundary-type-1234567892-alt";
        public static string BOUNDARY_START = "--boundary-type-1234567892-alt";
        public static string BOUNDARY_END = "--boundary-type-1234567892-alt--";
        public static string CONTENT_TYPE_CHARSET_CMD = "Content-Type: {0}; charset=\"{1}\";";
        public static string CONTENT_DISPOSITION_CMD = "Content-Disposition: {0}";
        public static string QUIT_CMD = "QUIT{0}";
        public static string CONTENT_TRANSFER_ENCODING_CMD = "Content-Transfer-Encoding: {0}";
        public static string CONTENT_TYPE_NAME_CMD = "Content-Type: {0};name=\"{1}\"";
        public static string CONTENT_DISPOSITION_FILENAME_CMD = "Content-Disposition: {0};filename=\"{1}\";";
        public static string END_MESSAGE_CMD = ".";
        public static string EOF = "\r\n";
        public static string CONTENT_TYPE_MULTIPART_MIXED = "multipart/mixed";
        public static string CONTENT_TYPE_TEXT_HTML = "text/html";
        public static string CONTENT_TYPE_TEXT_PLAIN = "text/plain";
        public static string CONTENT_TYPE_APPLICATION_OCTET = "application/octet-stream";
        public static string CHARSET_UTF8 = "UTF-8";
        public static string CONTENT_DISPOSITION_INLINE = "inline";
        public static string CONTENT_DISPOSITION_ATTACHMENT = "attachment";
        public static string CONTENT_TRANSFER_ENCODING_BASE64 = "base64";
        
    }
}
