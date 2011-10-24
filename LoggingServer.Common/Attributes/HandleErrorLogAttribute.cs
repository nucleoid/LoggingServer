using System.Web.Mvc;
using NLog;

namespace LoggingServer.Common.Attributes
{
    public class HandleErrorLogAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var typeName = filterContext.Exception.TargetSite.DeclaringType.FullName;
            LogManager.GetLogger(typeName).ErrorException(filterContext.Exception.Message, filterContext.Exception);
        }
    }
}
