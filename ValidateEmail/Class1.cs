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

            ValidateEmail validateEmail = new ValidateEmail();
            //validateEmail.Host = "mx.newgen.co.in";
            //validateEmail.Port = 25;
            //validateEmail.Protocol = "SMTP";
            //validateEmail.EnableSsl = false;
            //validateEmail.Username = "naveen-kumar";
            //validateEmail.Password = "sample";

            //validateEmail.Host = "smtp.gmail.com";
            //validateEmail.Port = 465;
            //validateEmail.Protocol = "SMTP";
            //validateEmail.EnableSsl = true;
            //validateEmail.Username = "snoopy.newgen";
            //validateEmail.Password = "sample";

            //validateEmail.Host = "smtp.gmail.com";
            //validateEmail.Port = 465;//587 TLS
            //validateEmail.Protocol = "SMTP";
            //validateEmail.EnableSsl = true;
            //validateEmail.Username = "snoopy.newgen";
            //validateEmail.Password = "sample";


            //validateEmail.Host = "pop-mail.outlook.com";
            //validateEmail.Port = 995;//587 TLS
            //validateEmail.Protocol = "POP";
            //validateEmail.EnableSsl = true;
            //validateEmail.Username = "naveen.bhatnagar@oulook.com";
            //validateEmail.Password = "sample";

            validateEmail.Host = "pop.gmail.com";
            validateEmail.Port = 995;//587 TLS
            validateEmail.Protocol = "POP";
            validateEmail.EnableSsl = true;
            validateEmail.Username = "snoopy.newgen";
            validateEmail.Password = "sample";
            validateEmail.Authenticate();

        }
    }
}
