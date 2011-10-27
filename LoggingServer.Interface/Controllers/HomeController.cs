using System.Web.Mvc;
using NLog;

namespace LoggingServer.Interface.Controllers
{
    public class HomeController : Controller
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Logs");
        }

        public ActionResult About()
        {
            _logger.Debug("testing debug log");
            return new ContentResult {Content = "Logged a test debug log message"};
        }
    }
}
