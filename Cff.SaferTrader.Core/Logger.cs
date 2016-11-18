using System;
using System.Diagnostics;
using System.Text;
using log4net;
using log4net.Config;

namespace Cff.SaferTrader.Core
{
    [Serializable]
    public class Logger : IDisposable
    {
        private readonly ILog log4NetLogger;
        private readonly EventLog eventLog;

        public Logger()
        {
            XmlConfigurator.Configure();
            log4NetLogger = LogManager.GetLogger("Cff.SaferTrader.Logger");

            eventLog = new EventLog("DebtorManagement");
            eventLog.Source = "DebtorManagement";
        }

        public void LogError(Exception e)
        {
            ArgumentChecker.ThrowIfNull(e, "e");
            log4NetLogger.Error(e);
            eventLog.WriteEntry(GenerateExceptionMessage(e).ToString(), EventLogEntryType.Error);
        }

        public void LogError(string message, Exception e)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(message, "message");
            log4NetLogger.Error(message, e);
            eventLog.WriteEntry(GenerateExceptionMessage(e).ToString(), EventLogEntryType.Error);
        }
        
        public void Debug(string message)
        {
            ArgumentChecker.ThrowIfNullOrEmpty(message, "message");
            log4NetLogger.Debug(message);
            eventLog.WriteEntry(message, EventLogEntryType.Information);
        }

        private static StringBuilder GenerateExceptionMessage(Exception exception)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(exception.Message);

            if (exception.InnerException != null)
            {
                builder.AppendLine("------------------------------");
                builder.AppendLine(exception.InnerException.Message);
            }

            builder.AppendLine("------------------------------");
            builder.AppendLine(exception.StackTrace);

            return builder;
        }

        public void Dispose() 
        { 
        
        }
    }
}
