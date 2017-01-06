using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateEmail.ImapConnector
{
    public static class NGImapCommands
    {
        public static string PREFIX_CMD = "$";
        public static string EOL = "\r\n";
        public static string STARTTLS_CMD = "{0} STARTTLS{1}";
        public static string CAPABILITY_CMD = "{0} CAPABILITY{1}";
        public static string LOGIN_CMD = "{0} LOGIN {1} {2}{3}";
    }
}
