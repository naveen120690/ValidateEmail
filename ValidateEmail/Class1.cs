using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ValidateEmail.ImapConnector;

namespace ValidateEmail
{
    public class Class1
    {
        const string IMAP_OK_SERVER_RESPONSE = "* OK";
        protected const string IMAP_COMMAND_PREFIX = "$";
        protected const string IMAP_CAPABILITY_COMMAND = "CAPABILITY";
        static void Main(string[] args)
        {
            System.Net.WebProxy myProxy = new System.Net.WebProxy("192.168.55.218", 8080);
            myProxy.Credentials = new System.Net.NetworkCredential("naveen-kumar", "sample");
            WebRequest.DefaultWebProxy = myProxy;

            Logger.Initialize(AppDomain.CurrentDomain.BaseDirectory, true);

            ValidateEmail validateEmail = new ValidateEmail();
            
            validateEmail.Host = "mx.newgen.co.in";
            validateEmail.Port = 25;
            validateEmail.Protocol = "SMTP";
            validateEmail.ConnectionSecurity = NGConnectionSecurity.None;
            validateEmail.Username = "username";
            validateEmail.Password = "password";

            validateEmail.Host = "mx.newgen.co.in";
            validateEmail.Port = 465;
            validateEmail.Protocol = "SMTP";
            validateEmail.ConnectionSecurity = NGConnectionSecurity.SSL_TLS;
            validateEmail.Username = "username";
            validateEmail.Password = "password";


            validateEmail.Host = "mx.newgen.co.in";
            validateEmail.Port = 587;
            validateEmail.Protocol = "SMTP";
            validateEmail.ConnectionSecurity = NGConnectionSecurity.None;
            validateEmail.Username = "username";
            validateEmail.Password = "password";

            validateEmail.Host = "smtp.gmail.com";
            validateEmail.Port = 25;
            validateEmail.Protocol = "SMTP";
            validateEmail.ConnectionSecurity = NGConnectionSecurity.STARTTLS;
            validateEmail.Username = "username";
            validateEmail.Password = "password";


            validateEmail.Host = "smtp.gmail.com";
            validateEmail.Port = 465;
            validateEmail.Protocol = "SMTP";
            validateEmail.ConnectionSecurity = NGConnectionSecurity.SSL_TLS;
            validateEmail.Username = "username";
            validateEmail.Password = "password";

            validateEmail.Host = "smtp.gmail.com";
            validateEmail.Port = 587;
            validateEmail.Protocol = "SMTP";
            validateEmail.ConnectionSecurity = NGConnectionSecurity.STARTTLS;
            validateEmail.Username = "username";
            validateEmail.Password = "password";

            validateEmail.Host = "smtp-mail.outlook.com";
            validateEmail.Port = 25;
            validateEmail.Protocol = "SMTP";
            validateEmail.ConnectionSecurity = NGConnectionSecurity.STARTTLS;
            validateEmail.Username = "username";
            validateEmail.Password = "password";

            validateEmail.Host = "smtp-mail.outlook.com";
            validateEmail.Port = 587;
            validateEmail.Protocol = "SMTP";
            validateEmail.ConnectionSecurity = NGConnectionSecurity.STARTTLS;
            validateEmail.Username = "username";
            validateEmail.Password = "password";

            //Imap Settings
            validateEmail.Host = "mx.newgen.co.in";
            validateEmail.Port = 995;//587 TLS
            validateEmail.Protocol = "IMAP";
            validateEmail.ConnectionSecurity = NGConnectionSecurity.SSL_TLS;
            validateEmail.Username = "username";
            validateEmail.Password = "password";

            validateEmail.Host = "imap.gmail.com";
            validateEmail.Port = 993;
            validateEmail.Protocol = "IMAP";
            validateEmail.ConnectionSecurity = NGConnectionSecurity.SSL_TLS;
            validateEmail.Username = "username";
            validateEmail.Password = "password";

            validateEmail.Host = "imap-mail.outlook.com";
            validateEmail.Port = 993;
            validateEmail.Protocol = "IMAP";
            validateEmail.ConnectionSecurity = NGConnectionSecurity.SSL_TLS;
            validateEmail.Username = "username";
            validateEmail.Password = "password";

            validateEmail.Host = "imap.gmx.com";
            validateEmail.Port = 143;
            validateEmail.Protocol = "IMAP";
            validateEmail.ConnectionSecurity = NGConnectionSecurity.STARTTLS;
            validateEmail.Username = "username";
            validateEmail.Password = "password";

            var authnticate=validateEmail.Authenticate();
            Console.Write(authnticate);

        }
    }
}
