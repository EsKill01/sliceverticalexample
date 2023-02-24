namespace TP.NA.CartService.Application.Abstractions.Logger
{
    /// <summary>
    /// Ilogger manager interface
    /// </summary>
    public interface ILoggerManager
    {
        void LogInfo(string message);

        void LogWarn(string message);

        void LogDebug(string message);

        void LogError(string message);
    }
}