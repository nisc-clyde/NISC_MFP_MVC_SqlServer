namespace NISC_MFP_MVC_Common.Logger
{
    public interface ILogHandler
    {
        ILogHandler setNext(ILogHandler logHandler);
        object LogHandle(string type, string operate, object data);
    }
}
