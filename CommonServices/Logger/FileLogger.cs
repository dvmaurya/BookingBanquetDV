using CommonServices.ConstantData;
using CommonServices.Extensions;
using MetroLog;
using MetroLog.Targets;
using System;

namespace CommonServices.Logger
{
    public class FileLogger : ILogger
    {
        private static readonly object _lock = new object();
        private static FileLogger _logger = null;
        private MetroLog.ILogger _metrologILogger = null;
        private int LogRetainDays = ApplicationConstants.LogRetainDays.ToInt32();

        public static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    lock (_lock)
                    {
                        if (_logger == null)
                        {
                            _logger = new FileLogger();
                        }
                    }
                }
                return _logger;
            }
        }

        private FileLogger()
        {
#if DEBUG
            LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Trace, LogLevel.Fatal, new StreamingFileTarget { RetainDays = LogRetainDays });

#else
            LogManagerFactory.DefaultConfiguration.AddTarget(LogLevel.Info, LogLevel.Fatal, new StreamingFileTarget { RetainDays = LogRetainDays });

#endif
            _metrologILogger = LogManagerFactory.DefaultLogManager.GetLogger("Banquet");
        }

        public void Debug(string message)
        {
            _metrologILogger.Debug(message);
        }

        public void Trace(string message)
        {
            _metrologILogger.Trace(message);
        }

        public void LogError(string message, Exception exception)
        {
            _metrologILogger.Error(message, exception);
        }

        public void Fatal(string message, Exception exception)
        {
            _metrologILogger.Fatal(message, exception);
        }

        public void Info(string message)
        {
            _metrologILogger.Info(message);
        }

        public void Warn(string message, Exception exception)
        {
            _metrologILogger.Warn(message, exception);
        }
    }
}
