using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ValidateEmail
{
    /// <summary>
    /// Class for logging 
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Default logfile Path
        /// </summary>
        private static string logFileName = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\" + "log\\" + "ValidateEmail";
        /// <summary>
        /// Default start log file number. example : NGEmailClient_0.log
        /// </summary>
        private static int logFileNo = 0;
        /// <summary>
        /// bool value to enable logging or not
        /// </summary>
        private static bool isLogging = false;
        /// <summary>
        /// Maximum log file size
        /// </summary>
        public static int MaxFileSizeMB = 5;
        /// <summary>
        /// Maximum number of log files generated.
        /// </summary>
        public static int MaxFileNumber = 5;
        /// <summary>
        /// To enable full message print in log file.
        /// </summary>
        public static bool EnableFullMessageLog = false;

        /// <summary>
        /// Static method to initialize logging in specified path.
        /// </summary>
        /// <param name="baseDirectory">base directory where log file is generated</param>
        /// <param name="isLogging1">Enable logging</param>
        public static void Initialize(string baseDirectory, bool isLogging1)
        {
            logFileName = baseDirectory + "App_Data\\" + "log\\" + "ValidateEmail";
            isLogging = isLogging1;
        }

        /// <summary>
        /// Print all exception with stacktrace in log file
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="exception">Exception to ptint</param>
        public static void Error(string message, Exception exception)
        {
            if (isLogging)
            {
                try
                {
                    var filename = GetFileName(logFileName);
                    var streamWriter = new System.IO.StreamWriter(filename, true);
                    streamWriter.WriteLine(DateTime.Now.ToString() + " " + message + "\n");
                    streamWriter.WriteLine(exception.Message + " " + exception.InnerException + " " + exception.StackTrace + "\n");
                    streamWriter.Close();
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }

        /// <summary>
        /// It generates and writes Debug logs with date and time in debug log files.
        /// </summary>
        /// <param name="message">Message</param>
        public static void Trace(string message)
        {
            if (isLogging)
            {
                var stackTrace = new System.Diagnostics.StackTrace();
                StringBuilder initial = new StringBuilder();
                var frames = stackTrace.GetFrames();
                foreach (var frame in frames)
                {
                    var methodBase = frame.GetMethod();
                    initial.Append("#" + methodBase.Name + ";" + methodBase.MethodHandle.Value + "#");
                }
                try
                {
                    if (message.StartsWith("Request command :From:", StringComparison.OrdinalIgnoreCase) && !EnableFullMessageLog)
                    {
                        return;
                    }
                    var filename = GetFileName(logFileName);
                    var streamWriter = new System.IO.StreamWriter(filename, true);
                    streamWriter.WriteLine(initial.ToString() + " " + DateTime.Now.ToString() + " " + message + "\n");
                    streamWriter.Close();
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }

        /// <summary>
        /// It generates and writes Debug logs with date and time in debug log files.
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="tag">Tag</param>
        public static void Trace(string tag, string message)
        {
            if (isLogging)
            {
                try
                {
                    if (message.StartsWith("From:", StringComparison.OrdinalIgnoreCase) && !EnableFullMessageLog)
                    {
                        return;
                    }
                    var filename = GetFileName(logFileName);
                    var streamWriter = new System.IO.StreamWriter(filename, true);
                    streamWriter.WriteLine("[" + tag + "] " + DateTime.Now.ToString() + " " + message + "\n");
                    streamWriter.Close();
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
            }
        }

        /// <summary>
        /// To generate runtime log file name.
        /// </summary>
        /// <param name="filename">Initial log file name</param>
        /// <returns>Generate logfile name</returns>
        public static string GetFileName(string filename)
        {
            var fileNo = 0;
            fileNo = logFileNo;
            var newFilename = filename + "_" + fileNo + ".log";
            try
            {
                if (System.IO.File.Exists(newFilename))
                {
                    FileInfo info = new FileInfo(newFilename);
                    if (info.Length > (1024 * 1024 * MaxFileSizeMB))
                    {
                        fileNo++;
                        if (fileNo > MaxFileNumber)
                        {
                            fileNo = 0;
                        }
                        newFilename = filename + "_" + fileNo + ".log";
                        if (File.Exists(newFilename))
                        {
                            try
                            {
                                File.Delete(newFilename);
                            }
                            catch (Exception e) { Console.WriteLine(e.Message); }
                        }
                        else
                        {
                            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(newFilename));
                        }
                        logFileNo = fileNo;
                    }
                }
                else
                {
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(newFilename));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return newFilename;
        }
    }
}
