using System.Threading;
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

        public ActionResult Stress()
        {
            for (int i = 0; i <100 ; i++)
            {
                _logger.Error("testing Error log");
                _logger.Warn("testing Warn log");
                _logger.Debug("testing Debug log");
            }
            return new ContentResult { Content = "<a href='/Home/Stress'>Stress again!</a>" };
        }

        public ActionResult DebugThis(int id)
        {
            for (int i = 0; i < id; i++)
            {
                _logger.Debug(i);
                Thread.Sleep(1);
            }
            return new ContentResult { Content = "<a href='/Home/DebugThis/1000'>DebugThousand!</a>" };
        }
    }
}
