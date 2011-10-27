using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using LoggingServer.Common.Extensions;
using LoggingServer.Interface.Models;
using LoggingServer.Server.Domain;
using LoggingServer.Server.Repository;
using LoggingServer.Server.Tasks;
using MvcContrib.UI.Grid;

namespace LoggingServer.Interface.Controllers
{
    public class LogsController : Controller
    {
        private readonly ILogEntryTasks _logEntryTasks;
        private readonly IReadableRepository<Project> _projectRepository;

        public LogsController(ILogEntryTasks logEntryTasks, IReadableRepository<Project> projectRepository)
        {
            _logEntryTasks = logEntryTasks;
            _projectRepository = projectRepository;
        }

        public ActionResult Index(SearchFilter filter, GridSortOptions gridSortOptions, int? page)
        {
            var projects = _projectRepository.All().ToList();
            return View(projects);
        }

        public ActionResult Search(SearchFilter filter, GridSortOptions gridSortOptions, int? page)
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
