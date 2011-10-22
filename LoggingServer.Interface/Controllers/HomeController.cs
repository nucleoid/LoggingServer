using System;
using System.Web.Mvc;
using NLog;

namespace LoggingServer.Interface.Controllers
{
    public class HomeController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            logger.Debug("testing debug log");
            return new ContentResult {Content = "Blah!"};
        }
    }
}
