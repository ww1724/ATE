using System;

namespace ATE.Service.Interface
{
    public interface ILoggerService
    {
        void Info(string Message);

        void Warn(string Message);

        void Error(string Message);

        void Error(string Message, Exception exception);
    }
}
