using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateEmail.PopConnector
{
    public static class NGPopStatusCode
    {
        public static string POP_OK = "+OK";
        public static string POP_OK_COMPLETED = NGPopCommands.PREFIX_CMD + " " + "OK";
        public static string POP_FAIL = "NOT OK";
        public static string POP_NO = NGPopCommands.PREFIX_CMD + " NO";
        public static string POP_BAD = "-ERR";
    }
}

