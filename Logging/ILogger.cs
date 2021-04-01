namespace Logging
{
    public interface ILogger
    {
        void Error(string message);
        void Debug(string message);
    }
}