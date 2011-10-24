using System;
using System.Web.Mvc;
using NLog;

namespace LoggingServer.Interface.Controllers
{
    public class HomeController : Controller
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            _logger.Debug("testing debug log");
            return new ContentResult {Content = "Blah!"};
        }

        public ActionResult ThrowException()
        {
            throw new Exception("just testing exception logging");
        }
    }
}
