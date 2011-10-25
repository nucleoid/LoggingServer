using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using LoggingServer.Interface.Models;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Tasks;
using MvcContrib.UI.Grid;

namespace LoggingServer.Interface.Controllers
{
    public class LogsController : Controller
    {
        private readonly ILogEntryTasks _logEntryTasks;

        public LogsController(ILogEntryTasks logEntryTasks)
        {
            _logEntryTasks = logEntryTasks;
        }

        public ActionResult Index(SearchFilter filter, GridSortOptions gridSortOptions, int? page)
        {
            var results = _logEntryTasks.Paged(page, null /*default pageSize*/, filter);

            var pagedModel = new PagedModel<LogEntry, LogEntryModel>
            {
                ViewData = ViewData,
                Query = results,
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "DateAdded",
                Page = page,
            }.Setup();
            return View(pagedModel);
        }
    }
}
