using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateEmail.PopConnector
{
    public static class NGPopCommands
    {
        public static string PREFIX_CMD = "$";
        public static string EOL = "\r\n";
        public static string STARTTLS_CMD = "STARTTLS{0}";
        public static string CAPABILITY_CMD = "{0} CAPABILITY{1}";
        public static string LOGIN_USER_CMD = "USER {0}{1}";
        public static string LOGIN_PASS_CMD = "PASS {0}{1}";
    }
}
