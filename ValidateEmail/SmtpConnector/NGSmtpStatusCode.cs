using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ValidateEmail.SmtpConnector
{
    /// <summary>
    /// It contains status code of response.
    /// </summary>
    public static class NGStatusCode
    {
        public static NGSmtpStatusCode code = NGSmtpStatusCode.GeneralFailure;

        public static int ToIntValue(this NGSmtpStatusCode statusCode)
        {
            return (int)statusCode;
        }
        public static string ToStringValue(this NGSmtpStatusCode statusCode)
        {
            return Convert.ToString((int)statusCode);
        }
    }
    /// <summary>
    /// Smtp Status code 
    /// </summary>
    public enum NGSmtpStatusCode
    {
        BadCommandSequence = 503,
        CannotVerifyUserWillAttemptDelivery = 252,
        ClientNotPermitted = 454,
        CommandNotImplemented = 502,
        CommandParameterNotImplemented = 504,
        CommandUnrecognized = 500,
        ExceededStorageAllocation = 552,
        GeneralFailure = -1,
        HelpMessage = 214,
        InsufficientStorage = 452,
        LocalErrorInProcessing = 451,
        MailboxBusy = 450,
        MailboxNameNotAllowed = 553,
        MailboxUnavailable = 550,
        Ok = 250,
        ServiceClosingTransmissionChannel = 221,
        ServiceNotAvailable = 421,
        ServiceReady = 220,
        StartMailInput = 354,
        SyntaxError = 501,
        SystemStatus = 211,
        TransactionFailed = 554,
        UserNotLocalTryAlternatePath = 551,
        UserNotLocalWillForward = 251,
        MustIssueStartTlsFirst = 530,
        NextAcceptBase64 = 334,
        Authenticated = 235,
        UserNotAuthenticated = 535,
    }
}

