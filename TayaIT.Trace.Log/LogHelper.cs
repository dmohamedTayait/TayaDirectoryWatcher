using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.ObjectBuilder2;
using System.Diagnostics;
using System.Web;

namespace TayaIT.Trace.Log
{
    public enum LogType
    {
        Crawler,
        Website,
        Watcher

    }
    public static class LogHelper
    {
        public static void LogException(Exception ex, string classFullQualifiedName, LogType logType)
        {
            if (ex != null)
            {
                LogEntry log = new LogEntry();
                log.EventId = 1;
                log.TimeStamp = DateTime.Now;
                log.Title = ex.Message;
                log.Categories.Add(logType.ToString());
                log.Categories.Add(classFullQualifiedName);
                log.Message = PrepareExceptionString(ex, logType);
                log.Severity = TraceEventType.Error;
                Logger.Write(log);
            }
        }
        public static void LogException(Exception ex, string classFullQualifiedName, LogType logType, string customMessage)
        {
            if (ex != null)
            {
                LogEntry log = new LogEntry();
                log.EventId = 1;
                log.TimeStamp = DateTime.Now;
                log.Title = ex.Message;
                log.Categories.Add(logType.ToString());
                log.Categories.Add(classFullQualifiedName);
                log.Message = customMessage + "\r\n+Exception Message:\r\n" + ex.Message + "\r\n+Exception Stack Trace:\r\n" + ex.StackTrace;
                log.Severity = TraceEventType.Error;
                Logger.Write(log);
            }
        }
        private static string PrepareExceptionString(Exception ex, LogType logType)
        {
            StringBuilder sbException = null;

            sbException = new StringBuilder();
            if (logType == LogType.Website)
                sbException.AppendFormat("<URL>{0}</URL>", HttpContext.Current.Request.Url.ToString());

            sbException.AppendFormat
                ("<SOURCE>{0}</SOURCE><STACKTRACE>{1}</STACKTRACE>",
                ex.Source,
                ex.StackTrace
                );

            if (ex.InnerException != null)
            {
                sbException.AppendFormat("<INNEREXCEPTION><INNERMESSAGE>{0}</INNERMESSAGE><SOURCE>{1}</SOURCE><STACKTRACE>{2}</STACKTRACE></INNEREXCEPTION>",
                    ex.InnerException.Message,
                    ex.InnerException.Source,
                    ex.InnerException.StackTrace);
            }

            return sbException.ToString();
        }

        public static void LogMessage(string message, string classFullQualifiedName, LogType logType, TraceEventType eventType)
        {
            LogEntry log = new LogEntry();
            log.EventId = 2;
            log.Message = message;
            log.Title = message;
            log.TimeStamp = DateTime.Now;
            log.Categories.Add(logType.ToString());
            log.Categories.Add(classFullQualifiedName);
            log.Severity = eventType;
            Logger.Write(log);
        }
    }
}
