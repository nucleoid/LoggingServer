using System.Web.Mvc;

namespace LoggingServer.Interface.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Excepted()
        {
            return View();
        }
    }
}