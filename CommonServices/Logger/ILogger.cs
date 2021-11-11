using System;

namespace CommonServices.Logger
{
    public interface ILogger
    {
        void Trace(string message);

        void Debug(string message);

        void LogError(string message, Exception exception);

        void Fatal(string message, Exception exception);

        void Info(string message);

        void Warn(string message, Exception exception);
    }
}
