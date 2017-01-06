using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateEmail.ImapConnector
{
    public static class NGImapStatusCode
    {
        public static string IMAP_OK = "* OK";
        public static string IMAP_OK_COMPLETED = NGImapCommands.PREFIX_CMD + " " + "OK";
        public static string IMAP_FAIL = "NOT OK";
        public static string IMAP_NO = NGImapCommands.PREFIX_CMD + " NO";
        public static string IMAP_BAD = NGImapCommands.PREFIX_CMD + " BAD";
    }
}

