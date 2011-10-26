using System;
using System.Web.Mvc;
using AutoMapper;
using LoggingServer.Common.Extensions;
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
            var totalCount = _logEntryTasks.Count(filter);
            var pagedModel = new PagedModel<LogEntry, LogEntrySummaryModel>
            {
                ViewData = ViewData,
                Query = results,
                GridSortOptions = gridSortOptions,
                DefaultSortColumn = "DateAdded",
                Page = page,
                Total = totalCount
            }.Setup();
            ViewData["filter.LogLevel"] = filter.LogLevel;
            return View(pagedModel);
        }

        public ActionResult Details(Guid id)
        {
            var log = _logEntryTasks.Get(id);
            var model = Mapper.Map<LogEntry, LogEntryModel>(log);
            return View(model);
        }
    }
}
